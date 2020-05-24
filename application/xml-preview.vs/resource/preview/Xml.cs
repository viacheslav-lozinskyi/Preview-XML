
using System.IO;
using System.Xml;

namespace resource
{
    namespace preview
    {
        public class Xml : cartridge.AnyPreview
        {
            protected override bool _IsEnabled(string url)
            {
                return Path.GetExtension(url).ToLower() == ".xml";
            }

            protected override bool _IsBest(string url)
            {
                return true;
            }

            protected override bool _Execute(string url, atom.Trace context)
            {
                var a_Context = new XmlDocument();
                {
                    a_Context.Load(url);
                }
                {
                    __Execute(url, a_Context.DocumentElement, 1, context);
                }
                return true;
            }

            private static void __Execute(string url, XmlNode node, int level, atom.Trace context)
            {
                if (node == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(node.Name))
                {
                    return;
                }
                if (context.IsTerminated() == false)
                {
                    if (node.NodeType != XmlNodeType.Comment)
                    {
                        context.
                            Clear().
                            SetContent(__GetContent(node)).
                            SetComment(__GetComment(node)).
                            SetPattern(__GetPattern(node)).
                            SetFlag((level == 1) ? cartridge.AnyPreview.NAME.FLAG.EXPAND : "").
                            SetHint("Tag type").
                            //SetUrl(url).
                            SetLevel(level).
                            Send();
                    }
                    if ((node.Attributes != null) && (node.NodeType == XmlNodeType.Element))
                    {
                        foreach (XmlAttribute a_Context in node.Attributes)
                        {
                            if (context.IsTerminated())
                            {
                                return;
                            }
                            {
                                __Execute(url, a_Context, level + 1, context);
                            }
                        }
                    }
                    if ((node.ChildNodes != null) && (node.NodeType == XmlNodeType.Element))
                    {
                        foreach (XmlNode a_Context in node.ChildNodes)
                        {
                            if (context.IsTerminated())
                            {
                                return;
                            }
                            {
                                __Execute(url, a_Context, level + 1, context);
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
                    case XmlNodeType.EntityReference: return "EntityReference";
                    case XmlNodeType.Entity: return "Entity";
                    case XmlNodeType.ProcessingInstruction: return "ProcessingInstruction";
                    case XmlNodeType.Comment: return "Comment";
                    case XmlNodeType.Document: return "Document";
                    case XmlNodeType.DocumentType: return "DocumentType";
                    case XmlNodeType.DocumentFragment: return "DocumentFragment";
                    case XmlNodeType.Notation: return "Notation";
                    case XmlNodeType.Whitespace: return "Whitespace";
                    case XmlNodeType.SignificantWhitespace: return "SignificantWhitespace";
                    case XmlNodeType.EndElement: return "EndElement";
                    case XmlNodeType.EndEntity: return "EndEntity";
                    case XmlNodeType.XmlDeclaration: return "XmlDeclaration";
                }
                return "";
            }

            private static string __GetContent(XmlNode node)
            {
                return node.Name + (string.IsNullOrEmpty(node.Value) ? "" : (" = " + cartridge.AnyUtility.GetCleanString(node.Value)));
            }

            private static string __GetPattern(XmlNode node)
            {
                return (node.NodeType == XmlNodeType.Attribute) ? cartridge.AnyPreview.NAME.PATTERN.PARAMETER : "";
            }

            //private static int __GetLine(string xml, int position)
            //{
            //    var a_Result = 0;
            //    if (string.IsNullOrEmpty(xml) == false)
            //    {
            //        var a_TextSize = xml.Length;
            //        for (var i = 0; (i < a_TextSize) && (i < position); i++)
            //        {
            //            if (xml[i] == '\r')
            //            {
            //                a_Result++;
            //            }
            //        }
            //        return a_Result + 1;
            //    }
            //    return a_Result;
            //}

            //private static int __GetPosition(string xml, int position)
            //{
            //    if (string.IsNullOrEmpty(xml) == false)
            //    {
            //        for (var i = position; i >= 0; i--)
            //        {
            //            if ((xml[i] == '\r') || (xml[i] == '\n'))
            //            {
            //                return position - i;
            //            }
            //        }
            //    }
            //    return 0;
            //}
        };
    }
}
