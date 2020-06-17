
using System.Xml;

namespace resource.preview
{
    public class XML : cartridge.AnyPreview
    {
        internal class NAME
        {
            public const string EXTENSION = ".XML";
            public const string HINT = "Tag type";
        }

        protected override void _Execute(atom.Trace context, string url)
        {
            var a_Context = new XmlDocument();
            {
                a_Context.Load(url);
            }
            {
                __Execute(a_Context.DocumentElement, 1, context);
            }
        }

        private static void __Execute(XmlNode node, int level, atom.Trace context)
        {
            if (node == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(node.Name))
            {
                return;
            }
            if (GetState() == STATE.EXECUTE)
            {
                if ((node.NodeType != XmlNodeType.Comment) && (string.IsNullOrEmpty(node.Name) == false))
                {
                    context.
                        Clear().
                        SetContent(node.Name).
                        SetValue(node.Value).
                        SetComment(__GetComment(node)).
                        SetPattern(__GetPattern(node)).
                        SetFlag((level == 1) ? atom.Trace.NAME.FLAG.EXPAND : "").
                        SetHint(NAME.HINT).
                        SetLevel(level).
                        Send();
                }
                if ((node.Attributes != null) && (node.NodeType == XmlNodeType.Element))
                {
                    foreach (XmlAttribute a_Context in node.Attributes)
                    {
                        if (GetState() != STATE.EXECUTE)
                        {
                            return;
                        }
                        {
                            __Execute(a_Context, level + 1, context);
                        }
                    }
                }
                if ((node.ChildNodes != null) && (node.NodeType == XmlNodeType.Element))
                {
                    foreach (XmlNode a_Context in node.ChildNodes)
                    {
                        if (GetState() != STATE.EXECUTE)
                        {
                            return;
                        }
                        {
                            __Execute(a_Context, level + 1, context);
                        }
                    }
                }
            }
        }

        private static string __GetComment(XmlNode node)
        {
            switch (node.NodeType)
            {
                case XmlNodeType.None: return "None";
                case XmlNodeType.Element: return "Element";
                case XmlNodeType.Attribute: return "Attribute";
                case XmlNodeType.Text: return "Text";
                case XmlNodeType.CDATA: return "CDATA";
                case XmlNodeType.EntityReference: return "Entity Reference";
                case XmlNodeType.Entity: return "Entity";
                case XmlNodeType.ProcessingInstruction: return "Processing Instruction";
                case XmlNodeType.Comment: return "Comment";
                case XmlNodeType.Document: return "Document";
                case XmlNodeType.DocumentType: return "Document Type";
                case XmlNodeType.DocumentFragment: return "Document Fragment";
                case XmlNodeType.Notation: return "Notation";
                case XmlNodeType.Whitespace: return "Whitespace";
                case XmlNodeType.SignificantWhitespace: return "Significant Whitespace";
                case XmlNodeType.EndElement: return "End Element";
                case XmlNodeType.EndEntity: return "End Entity";
                case XmlNodeType.XmlDeclaration: return "Declaration";
            }
            return "";
        }

        private static string __GetPattern(XmlNode node)
        {
            if (node.NodeType == XmlNodeType.Attribute)
            {
                return atom.Trace.NAME.PATTERN.PARAMETER;
            }
            if ((node.NodeType == XmlNodeType.Element) && (node.ChildNodes != null) && (node.ChildNodes.Count > 0))
            {
                return "";
            }
            if ((node.NodeType == XmlNodeType.Element) && (node.Attributes != null) && (node.Attributes.Count > 0))
            {
                return "";
            }
            return atom.Trace.NAME.PATTERN.VARIABLE;
        }
    };
}
