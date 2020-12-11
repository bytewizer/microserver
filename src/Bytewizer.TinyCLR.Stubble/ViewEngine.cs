using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Reflection;

//using GHIElectronics.TinyCLR.Data.Json;

namespace Bytewizer.TinyCLR.Stubble
{
    public class ViewEngine
    {
        public IList Elements; //public List<ViewElement> Elements;
        public ArrayList Partials = new ArrayList(); //public List<ViewPartial> Partials = new List<ViewPartial>();
        
        public string Filename = "";
        public string HTML = "";
        public string Section = ""; //section of the template to use for rendering

        private ViewData data;
        
        /// <summary>
        /// Use a template file to bind data and replace mustache variables with data, e.g. {{my-name}} is replaced with value of View["my-name"]
        /// </summary>
        /// <param name="file">relative path to the template file</param>
        /// <param name="cache">Dictionary object used to save cached, parsed template to</param>
        public ViewEngine(string file, IDictionary cache = null) //public View(string file, Dictionary<string, SerializedView> cache = null)
        {
            Parse(file, "", "", cache);
        }

        /// <summary>
        /// Use a template file to bind data and replace mustache variables with data, e.g. {{my-name}} is replaced with value of View["my-name"]
        /// </summary>
        /// <param name="file">relative path to the template file</param>
        /// <param name="section">section name within the template file to load, e.g. {{my-section}} ... {{/my-section}}</param>
        /// <param name="cache">Dictionary object used to save cached, parsed template to</param>
        public ViewEngine(string file, string section, IDictionary cache = null)
        {
            if (section == null)
            {
                section = string.Empty;
            }
            
            Parse(file, section.ToLower(), "", cache);
        }

        public string this[string key]
        {
            get
            {
                if (!data.ContainsKey(key.ToLower()))
                {
                    data.Add(key.ToLower(), "");
                }
                return data[key.ToLower()];
            }
            set
            {
                data[key.ToLower()] = value;
            }
        }

        private void Parse(string file, string section = "", string html = "", IDictionary cache = null, bool loadPartials = true)
        {
            SerializedView cached = new SerializedView() { Elements = new ArrayList() };
            Filename = file;
            data = new ViewData();
            Section = section;
            if (file != "")
            {
#if (!DEBUG)
                if (cache == null && ViewCache.Cache != null)
                {
                    cache = ViewCache.Cache;
                }
#endif

                if (cache != null)
                {
                    if (cache.Contains(file + '/' + section) == true)
                    {
                        cached = (SerializedView)cache[file + '/' + section];
                        data = cached.Data;
                        Elements = cached.Elements;
                        //Fields = cached.Fields;
                    }
                }
            }

            if (cached.Elements.Count == 0)
            {
                Elements = new ArrayList();

                //try loading file from disk
                if (file != "")
                {
                    if (File.Exists(MapPath(file)))
                    {
                        var bytes = File.ReadAllBytes(MapPath(file));
                        HTML = Encoding.UTF8.GetString(bytes);
                        //HTML = File.ReadAllText(MapPath(file));
                    }
                }
                else
                {
                    HTML = html;
                }
                if (HTML.Trim() == "") { return; }

                //next, find the group of code matching the view section name
                if (section != "")
                {
                    //find starting tag (optionally with arguments)
                    //for example: {{button (name:submit, style:outline)}}
                    int[] e = new int[3];
                    e[0] = HTML.IndexOf("{{" + section);
                    if (e[0] >= 0)
                    {
                        e[1] = HTML.IndexOf("}", e[0]);
                        if (e[1] - e[0] <= 256)
                        {
                            e[1] = HTML.IndexOf("{{/" + section + "}}", e[1]);
                        }
                        else { e[0] = -1; }
                    }

                    if (e[0] >= 0 & e[1] > (e[0] + section.Length + 4))
                    {
                        e[2] = e[0] + 4 + section.Length;
                        HTML = HTML.Substring(e[2], e[1] - e[2]);
                    }
                }

                //get view from html code
                var dirty = true;
                while (dirty == true)
                {          
                    dirty = false;
                    //var arr = HTML.Split("{{");
                    var arr = Split(HTML, "{{");
                    var i = 0;
                    var s = 0;
                    var c = 0;
                    var u = 0;
                    var u2 = 0;
                    ViewElement viewElem;

                    //types of view elements

                    // {{title}}                        = variable
                    // {{address}} {{/address}}         = block
                    // {{button "/ui/button-medium"}}   = HTML include
                    // {{button "/ui/button" title:"save", onclick="do.this()"}} = HTML include with variables

                    //first, load all HTML includes
                    for (var x = 0; x < arr.Length; x++)
                    {
                        if (x == 0 && HTML.IndexOf(arr[x]) == 0)
                        {
                            arr[x] = "{!}" + arr[x];
                        }
                        else if (arr[x].Trim() != "")
                        {
                            i = arr[x].IndexOf("}}");
                            s = arr[x].IndexOf(':');
                            u = arr[x].IndexOf('"');
                            if (i > 0 && u > 0 && u < i - 2 && (s == -1 || s > u) && loadPartials == true)
                            {
                                //read partial include & load HTML from another file
                                viewElem.Name = arr[x].Substring(0, u - 1).Trim().ToLower();
                                u2 = arr[x].IndexOf('"', u + 2);
                                var partial_path = arr[x].Substring(u + 1, u2 - u - 1);

                                //load the view HTML
                                var newScaff = new ViewEngine(partial_path, "", cache);

                                //check for HTML include variables
                                if (i - u2 > 0)
                                {
                                    var vars = arr[x].Substring(u2 + 1, i - (u2 + 1)).Trim();
                                    if (vars.IndexOf(":") > 0)
                                    {
                                        //HTML include variables exist
                                        try
                                        {
                                            //var kv = (IDictionary)JsonConverter.Deserialize("{" + vars + "}");
                                            //var kv =  JsonSerializer.Deserialize<IDictionary>("{" + vars + "}");
                                            //foreach (DictionaryEntry kvp in kv)
                                            //{
                                            //    newScaff[(string)kvp.Key] = (string)kvp.Value;
                                            //}
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }

                                //rename child view variables, adding a prefix
                                var ht = newScaff.Render(newScaff.data, false);
                                var y = 0;
                                var prefix = viewElem.Name + "-";
                                while (y >= 0)
                                {
                                    y = ht.IndexOf("{{", y);
                                    if (y < 0) { break; }
                                    if (ht.Substring(y + 2, 1) == "/")
                                    {
                                        ht = ht.Substring(0, y + 3) + prefix + ht.Substring(y + 3);
                                    }
                                    else
                                    {
                                        ht = ht.Substring(0, y + 2) + prefix + ht.Substring(y + 2);
                                    }
                                    y += 2;
                                }

                                Partials.Add(new ViewPartial() { Name = viewElem.Name, Path = partial_path, Prefix = prefix });

                                //Partials.AddRange(newScaff.Partials.Select(a =>
                                //{
                                //    var partial = a;
                                //    partial.Prefix = prefix + partial.Prefix;
                                //    return partial;
                                //}));

                                arr[x] = "{!}" + ht + arr[x].Substring(i + 2);
                                HTML = JoinHTML(arr);
                                dirty = true; //HTML is dirty, restart loop
                                break;
                            }
                        }

                    }
                    if (dirty == false)
                    {
                        //next, process variables & blocks
                        for (var x = 0; x < arr.Length; x++)
                        {
                            if (x == 0 && HTML.IndexOf(arr[0].Substring(3)) == 0)//skip "{!}" using substring
                            {
                                //first element is HTML only
                                Elements.Add(new ViewElement() { Htm = arr[x].Substring(3), Name = "" });
                            }
                            else if (arr[x].Trim() != "")
                            {
                                i = arr[x].IndexOf("}}");
                                s = arr[x].IndexOf(' ');
                                c = arr[x].IndexOf(':');
                                u = arr[x].IndexOf('"');
                                viewElem = new ViewElement();
                                if (i > 0)
                                {
                                    viewElem.Htm = arr[x].Substring(i + 2);

                                    //get variable name
                                    if (s < i && s > 0)
                                    {
                                        //found space
                                        viewElem.Name = arr[x].Substring(0, s).Trim().ToLower();
                                    }
                                    else
                                    {
                                        //found tag end
                                        viewElem.Name = arr[x].Substring(0, i).Trim().ToLower();
                                    }

                                    if (viewElem.Name.IndexOf('/') < 0)
                                    {
                                        if (data.Fields.Contains(viewElem.Name))
                                        {
                                            //add element index to existing field
                                            int[] field = (int[])data.Fields[viewElem.Name];

                                            foreach (var item in field.Append(Elements.Count))
                                            {
                                                data.Fields[viewElem.Name] = item;
                                            }

                                            //TODO: Not tested
                                            //Fields[viewElem.Name] = field.Append(Elements.Count).ToArray();
                                        }
                                        else
                                        {
                                            //add field with element index
                                            data.Fields.Add(viewElem.Name, new int[] { Elements.Count });
                                        }
                                    }

                                    //get optional path stored within variable tag (if exists)
                                    //e.g. {{my-component "list"}}
                                    if (u > 0 && u < i - 2 && (c == -1 || c > u))
                                    {
                                        u2 = arr[x].IndexOf('"', u + 2);
                                        if (i - u2 > 0)
                                        {
                                            var data = arr[x].Substring(u + 1, u2 - u - 1);
                                            viewElem.Path = data;
                                        }
                                    }
                                    else if (s < i && s > 0)
                                    {
                                        //get optional variables stored within tag
                                        var vars = arr[x].Substring(s + 1, i - s - 1);
                                        try
                                        {
                                            //TODO: viewElem.Vars = (IDictionary)JsonConverter.Deserialize("{" + vars + "}");
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                                else
                                {
                                    viewElem.Name = "";
                                    viewElem.Htm = arr[x];
                                }
                                Elements.Add(viewElem);
                            }
                        }
                    }
                }

                //cache the view data
                if (cache != null)
                {
                    var view = new SerializedView
                    {
                        Data = data,
                        Elements = Elements,
                        Fields = data.Fields,
                        Partials = Partials
                    };
                    cache.Add(file + '/' + section, view);
                }
            }
        }

        private string JoinHTML(string[] html)
        {
            for (var x = 0; x < html.Length; x++)
            {
                if (html[x].Substring(0, 3) == "{!}")
                {
                    html[x] = html[x].Substring(3);
                }
                else
                {
                    html[x] = "{{" + html[x];
                }
            }

            var result = new StringBuilder();
            foreach (var item in html)
            {
                result.Append(item);
            }

            return result.ToString();

            //return string.Join("", html);
        }

        public string Render(ViewData nData, bool hideElements = true)
        {
            //deserialize list of elements since we will be manipulating the list,
            //so we don't want to permanently mutate the public elements array
            IList elems = DeepCopy((ArrayList)Elements);
            if (elems.Count > 0)
            {
                //render view with paired nData data
                var view = new StringBuilder();

                var closing = new ArrayList(); //var closing = new List<ClosingElement>();
                //remove any unwanted blocks of HTML from view
                for (var x = 0; x < elems.Count; x++)
                {
                    if (x < elems.Count - 1)
                    {
                        for (var y = x + 1; y < elems.Count; y++)
                        {
                            //check for closing tag
                            if (((ViewElement)elems[y]).Name == "/" + ((ViewElement)elems[x]).Name)
                            {
                                //add enclosed group of HTML to list for removing
                                var closed = new ClosingElement()
                                {
                                    Name = ((ViewElement)elems[x]).Name,
                                    Start = x,
                                    End = y
                                };

                                if (nData.ContainsKey(((ViewElement)elems[x]).Name) == true)
                                {
                                    //check if user wants to include HTML 
                                    //that is between start & closing tag  
                                    if (nData[((ViewElement)elems[x]).Name, true] == true)
                                    {
                                        closed.Show.Add(true);
                                    }
                                    else
                                    {
                                        closed.Show.Add(false);
                                    }
                                }
                                else
                                {
                                    closed.Show.Add(false);
                                }

                                closing.Add(closed);
                                break;
                            }
                        }
                    }
                }

                if (hideElements == true)
                {
                    //remove all groups of HTML in list that should not be displayed
                    IList removeIndexes = new ArrayList();
                    bool isInList = false;
                    for (int x = 0; x < closing.Count; x++)
                    {
                        if ((bool)((ClosingElement)closing[x]).Show[0] != true)
                        {
                            //add range of indexes from closing to the removeIndexes list
                            for (int y = ((ClosingElement)closing[x]).Start; y < ((ClosingElement)closing[x]).End; y++)
                            {
                                isInList = false;
                                for (int z = 0; z < removeIndexes.Count; z++)
                                {
                                    if ((int)removeIndexes[z] == y) { isInList = true; break; }
                                }
                                if (isInList == false) { removeIndexes.Add(y); }
                            }
                        }
                    }

                    //physically remove HTML list items from view
                    int offset = 0;
                    for (int z = 0; z < removeIndexes.Count; z++)
                    {
                        elems.RemoveAt((int)removeIndexes[z] - offset);
                        offset += 1;
                    }
                }

                //finally, replace view variables with custom data
                for (var x = 0; x < elems.Count; x++)
                {
                    //check if view item is an enclosing tag or just a variable
                    var useView = true;
                    if (((ViewElement)elems[x]).Name.IndexOf('/') < 0)
                    {
                        for (int y = 0; y < closing.Count; y++)
                        {
                            if (((ViewElement)elems[x]).Name == ((ClosingElement)closing[y]).Name)
                            {
                                useView = false; break;
                            }
                        }
                    }
                    else { useView = false; }

                    if ((nData.ContainsKey(((ViewElement)elems[x]).Name) == true
                    || ((ViewElement)elems[x]).Name.IndexOf('/') == 0) & useView == true)
                    {
                        //inject string into view variable
                        var s = nData[((ViewElement)elems[x]).Name.Replace("/", "")];
                        if (string.IsNullOrEmpty(s) == true) { s = ""; }
                        view.Append(s + ((ViewElement)elems[x]).Htm);
                    }
                    else
                    {
                        //passively add htm, ignoring view variable
                        view.Append((hideElements == true ? "" : (((ViewElement)elems[x]).Name != "" ? "{{" + ((ViewElement)elems[x]).Name + "}}" : "")) + ((ViewElement)elems[x]).Htm);
                    }
                }

                //render scaffolding as HTML string
                return view.ToString();
            }
            return "";
        }

        public string Get(string name)
        {
            var index = Elements.IndexOf(name); //var index = Elements.FindIndex(c => c.Name == name);
            
            if (index < 0) { return ""; }

            var part = (ViewElement)Elements[index];
            var html = part.Htm;
            for (var x = index + 1; x < Elements.Count; x++)
            {
                part = (ViewElement)Elements[x];
                if (part.Name == "/" + name) { break; }

                //add inner view elements
                if (part.Name.IndexOf('/') < 0)
                {
                    if (data.ContainsKey(part.Name))
                    {
                        if (data[part.Name, true] == true)
                        {
                            html += Get(part.Name);
                        }
                    }
                }
                else
                {
                    html += part.Htm;
                }

            }

            return html;
        }

        private static ArrayList DeepCopy(ArrayList obj)
        {
            if (!typeof(ArrayList).IsSerializable)
            {
                throw new Exception("The source object must be serializable");
            }

            if (obj == null)
            {
                throw new Exception("The source object must not be null");
            }

            ArrayList result = new ArrayList();
            foreach (ViewElement element in obj)
            {
                result.Add(element);
            }

            return result;
        }

        private static string MapPath(string strPath = "")
        {
            //var path = Path.GetFullPath(".").Replace("/", "/");
            //var path2 = strPath.Replace("\\", "/");
            //if (path2.Substring(0, 1) == "/") { path2 = path2.Substring(1); }
            //return Path.Combine(path, path2);
            return strPath;
        }

        private static string[] Split(string s, string delim)
        {
            if (s == null) throw new NullReferenceException();

            // Declarations
            var strings = new ArrayList();
            var start = 0;

            // Tokenize
            if (delim != null && delim != "")
            {
                int i;
                while ((i = s.IndexOf(delim, start)) != -1)
                {
                    strings.Add(s.Substring(start, i - start));
                    start = i + delim.Length;
                }
            }

            // Append left over
            strings.Add(s.Substring(start));

            return (string[])strings.ToArray(typeof(string));
        }
    }
}
