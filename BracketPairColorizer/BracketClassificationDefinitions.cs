using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace BracketPairColorizer
{

    internal static class BracketClassificationDefinitions
    {
        #region Classification Type Definitions

        [Export]
        [Name("0")]
        internal static ClassificationTypeDefinition bracketClassificationDefinitions = null;

        [Export]
        [Name("1")]
        [BaseDefinition("0")]
        internal static ClassificationTypeDefinition _1Definition = null;

        [Export]
        [Name("2")]
        [BaseDefinition("0")]
        internal static ClassificationTypeDefinition _2Definition = null;

        [Export]
        [Name("3")]
        [BaseDefinition("0")]
        internal static ClassificationTypeDefinition _3Definition = null;

        [Export]
        [Name("4")]
        [BaseDefinition("0")]
        internal static ClassificationTypeDefinition _4Definition = null;

        [Export]
        [Name("5")]
        [BaseDefinition("0")]
        internal static ClassificationTypeDefinition _5Definition = null;

        [Export]
        [Name("6")]
        [BaseDefinition("0")]
        internal static ClassificationTypeDefinition _6Definition = null;

        [Export]
        [Name("7")]
        [BaseDefinition("0")]
        internal static ClassificationTypeDefinition _7Definition = null;

        [Export]
        [Name("8")]
        [BaseDefinition("0")]
        internal static ClassificationTypeDefinition _8Definition = null;

        [Export]
        [Name("9")]
        [BaseDefinition("0")]
        internal static ClassificationTypeDefinition _9Definition = null;

        #endregion

        #region Classification Format Productions

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "1")]
        [Name("1")]
        [UserVisible(true)]
        [Order(After = Priority.High)]
        internal sealed class _1Format : ClassificationFormatDefinition
        {
            public _1Format()
            {
                ForegroundColor = Color.FromRgb(255, 0, 0);
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "2")]
        [Name("2")]
        [UserVisible(true)]
        [Order(After = Priority.High)]
        internal sealed class _2Format : ClassificationFormatDefinition
        {
            public _2Format()
            {
                ForegroundColor = Color.FromRgb(0, 0, 255);
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "3")]
        [Name("3")]
        [UserVisible(true)]
        [Order(After = Priority.High)]
        internal sealed class _3Format : ClassificationFormatDefinition
        {
            public _3Format()
            {
                ForegroundColor = Color.FromRgb(0, 255, 0);
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "4")]
        [Name("4")]
        [UserVisible(true)]
        [Order(After = Priority.High)]
        internal sealed class _4Format : ClassificationFormatDefinition
        {
            public _4Format()
            {
                ForegroundColor = Color.FromRgb(255, 0, 255);
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "5")]
        [Name("5")]
        [UserVisible(true)]
        [Order(After = Priority.High)]
        internal sealed class _5Format : ClassificationFormatDefinition
        {
            public _5Format()
            {
                ForegroundColor = Color.FromRgb(0, 0, 0);
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "6")]
        [Name("6")]
        [UserVisible(true)]
        [Order(After = Priority.High)]
        internal sealed class _6Format : ClassificationFormatDefinition
        {
            public _6Format()
            {
                ForegroundColor = Color.FromRgb(172, 0, 146);
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "7")]
        [Name("7")]
        [UserVisible(true)]
        [Order(After = Priority.High)]
        internal sealed class _7Format : ClassificationFormatDefinition
        {
            public _7Format()
            {
                ForegroundColor = Color.FromRgb(178, 101, 0);
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "8")]
        [Name("8")]
        [UserVisible(true)]
        [Order(After = Priority.High)]
        internal sealed class _8Format : ClassificationFormatDefinition
        {
            public _8Format()
            {
                ForegroundColor = Color.FromRgb(0, 125, 115);
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "9")]
        [Name("9")]
        [UserVisible(true)]
        [Order(After = Priority.High)]
        internal sealed class _9Format : ClassificationFormatDefinition
        {
            public _9Format()
            {
                ForegroundColor = Color.FromRgb(100, 23, 206);
            }
        }
        #endregion
    }
}
