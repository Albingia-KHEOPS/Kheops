using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Hexavia.Tools.Validators
{
    public static class XPathGeolocValidation
    {
        public static XmlDocument ValidateFile(string filePath)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            XmlReader reader = null;
            XmlDocument document = null;
            using (Stream schemaStream = myAssembly.GetManifestResourceStream("Hexavia.Tools.Validators.ContractGeolocValidator.xsd"))
            {
                try
                {

                    XmlSchema schema = XmlSchema.Read(schemaStream, null);

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.Schemas.Add(schema);
                    settings.ValidationType = ValidationType.Schema;

                    reader = XmlReader.Create(filePath, settings);
                    document = new XmlDocument();
                    document.Load(reader);

                    ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

                    // the following call to Validate succeeds.
                    document.Validate(eventHandler);

                    return document;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
                
            }
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    throw new Exception(string.Format("Error: {0}", e.Message));
                    break;
                case XmlSeverityType.Warning:
                    throw new Exception(string.Format("Warning: {0}", e.Message));
                    break;
            }

        }
    }
}
