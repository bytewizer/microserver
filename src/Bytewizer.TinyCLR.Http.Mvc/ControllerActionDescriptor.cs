using System;
using System.Reflection;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    [DebuggerDisplay("{DisplayName}")]
    public class ControllerActionDescriptor : ActionDescriptor
    {
        public string ControllerName { get; set; }

        public virtual string ActionName { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public override string DisplayName
        {
            get
            {
                if (base.DisplayName == null && MethodInfo != null)
                {
                    base.DisplayName = $"{MethodInfo.Name}";
                }

                return base.DisplayName;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                base.DisplayName = value;
            }
        }
    }
}
