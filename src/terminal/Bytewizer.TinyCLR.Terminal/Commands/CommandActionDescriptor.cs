using System;
using System.Reflection;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// A descriptor for an action of a command.
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public class CommandActionDescriptor : ActionDescriptor
    {
        /// <summary>
        /// The <see cref="MethodInfo"/>.
        /// </summary>
        public MethodInfo MethodInfo { get; set; }

        /// <summary>
        /// The friendly name of the command.
        /// </summary>
        public virtual string CommandName { get; set; }

        /// <summary>
        /// The friendly name of the action.
        /// </summary>
        public virtual string ActionName { get; set; }

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