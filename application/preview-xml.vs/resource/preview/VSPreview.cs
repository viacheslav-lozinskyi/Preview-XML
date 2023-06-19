using System.IO;
using System.Xml;

namespace resource.preview
{
    internal class VSPreview : extension.AnyPreview
    {
        protected override void _Execute(atom.Trace context, int level, string url, string file)
        {
            var a_Context = new XmlDocument();
            {
                a_Context.Load(file);
            }
            {
                __Execute(context, level, a_Context.DocumentElement);
            }
        }

        private static void __Execute(atom.Trace context, int level, XmlNode data)
        {
            if (data == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(data.Name))
            {
                return;
            }
            if (GetState() == NAME.STATE.WORK.CANCEL)
            {
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(data.Name) == false)
                {
                    if ((data.NodeType != XmlNodeType.Comment) && __IsContentFound(data))
                    {
                        context.
                            SetTrace(null, (level == 1) ? NAME.STATE.TRACE.EXPAND : NAME.STATE.TRACE.NONE).
                            SetComment(__GetComment(data), "[[[Data Type]]]").
                            Send(NAME.SOURCE.PREVIEW, __GetType(data), level, data.Name, __GetValue(data));
                    }
                }
                if ((data.Attributes != null) && (data.NodeType == XmlNodeType.Element))
                {
                    foreach (XmlAttribute a_Context in data.Attributes)
                    {
                        if (GetState() == NAME.STATE.WORK.CANCEL)
                        {
                            return;
                        }
                        else
                        {
                            __Execute(context, level + 1, a_Context);
                        }
                    }
                }
                if ((data.ChildNodes != null) && (data.NodeType == XmlNodeType.Element))
                {
                    foreach (XmlNode a_Context in data.ChildNodes)
                    {
                        if (GetState() == NAME.STATE.WORK.CANCEL)
                        {
                            return;
                        }
                        else
                        {
                            __Execute(context, level + 1, a_Context);
                        }
                    }
                }
            }
        }

        private static bool __IsContentFound(XmlNode data)
        {
            if (data.Name == "#text")
            {
                var a_Context = data.ParentNode;
                if ((a_Context.Attributes != null) && (a_Context.Attributes.Count > 0))
                {
                    return true;
                }
                if ((a_Context.ChildNodes != null) && (a_Context.ChildNodes.Count == 1))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool __IsChildrenFound(XmlNode data)
        {
            if ((data.Attributes != null) && (data.Attributes.Count > 0))
            {
                return true;
            }
            if (data.ChildNodes != null)
            {
                if (data.ChildNodes.Count != 1)
                {
                    return true;
                }
                if (data.ChildNodes[0].Name == "#text")
                {
                    return false;
                }
            }
            return true;
        }

        private static string __GetComment(XmlNode data)
        {
            switch (data.NodeType)
            {
                case XmlNodeType.None: return "[[[None]]]";
                case XmlNodeType.Element: return "[[[Element]]]";
                case XmlNodeType.Attribute: return "[[[Attribute]]]";
                case XmlNodeType.Text: return "[[[Text]]]";
                case XmlNodeType.CDATA: return "CDATA";
                case XmlNodeType.EntityReference: return "[[[Entity Reference]]]";
                case XmlNodeType.Entity: return "[[[Entity]]]";
                case XmlNodeType.ProcessingInstruction: return "[[[Processing Instruction]]]";
                case XmlNodeType.Comment: return "[[[Comment]]]";
                case XmlNodeType.Document: return "[[[Document]]]";
                case XmlNodeType.DocumentType: return "[[[Document Type]]]";
                case XmlNodeType.DocumentFragment: return "[[[Document Fragment]]]";
                case XmlNodeType.Notation: return "[[[Notation]]]";
                case XmlNodeType.XmlDeclaration: return "[[[Declaration]]]";
            }
            return "";
        }

        private static string __GetValue(XmlNode data)
        {
            if (string.IsNullOrWhiteSpace(data.Value))
            {
                return __IsChildrenFound(data) ? "" : GetFinalText(data.InnerText);
            }
            else
            {
                return GetFinalText(data.Value);
            }
        }

        private static string __GetType(XmlNode data)
        {
            if (data.NodeType == XmlNodeType.Attribute)
            {
                return NAME.EVENT.PARAMETER;
            }
            if ((data.Attributes != null) && (data.Attributes.Count > 0))
            {
                return NAME.EVENT.PARAMETER;
            }
            if ((data.ChildNodes != null) && (data.ChildNodes.Count > 0))
            {
                return __IsChildrenFound(data) ? NAME.EVENT.PARAMETER : NAME.EVENT.VARIABLE;
            }
            return NAME.EVENT.VARIABLE;
        }
    };
}
