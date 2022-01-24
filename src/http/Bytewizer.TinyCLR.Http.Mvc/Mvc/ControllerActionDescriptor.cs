using System;
using System.Reflection;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// A descriptor for an action of a controller.
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public class ControllerActionDescriptor : ActionDescriptor
    {
        /// <summary>
        /// The name of the controller.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// The name of the action.
        /// </summary>
        public virtual string ActionName { get; set; }

        /// <summary>
        /// The <see cref="MethodInfo"/>.
        /// </summary>
        public MethodInfo MethodInfo { get; set; }

        /// <inheritdoc />
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
