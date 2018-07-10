using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace BracketPairColorizer
{

    //[Export(typeof(IClassifierProvider))]
    //[ContentType("CSharp")]
    //internal class BracketClassifierProvider : IClassifierProvider
    //{
    //    [Import]
    //    internal IClassificationTypeRegistryService ClassificationRegistry = null;

    //    static BracketClassifier bracketClassifier;

    //    public IClassifier GetClassifier(ITextBuffer buffer)
    //    {
    //        if (bracketClassifier == null)
    //            bracketClassifier = new BracketClassifier(ClassificationRegistry);

    //        return bracketClassifier;
    //    }
    //}

    [Export(typeof(ITaggerProvider))]
    [ContentType("CSharp")]
    [TagType(typeof(IClassificationTag))]
    internal class BracketTaggerProvider : ITaggerProvider
    {
        [Import]
        internal IClassificationTypeRegistryService ClassificationRegistry = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return (ITagger<T>)new BracketTagger(buffer, ClassificationRegistry);
        }
    }
}
