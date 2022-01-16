using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Albingia.Kheops.OP.IncludeBusiness {
    class Program {
        const string projectExt = ".csproj";
        const string svcExt = ".svc";
        const string csharpExt = ".cs";
        enum CsProjKeyword {
            ItemGroup,
            Content,
            Compile,
            Include
        }
        enum LabelSuffix {
            Port,
            Service,
            Repository
        }
        enum WCFKeyword {
            service,
            name,
            endpoint,
            address,
            contract,
            behaviorConfiguration,
            bindingConfiguration
        }
        readonly static string[] pathList = new[] {
            @"OP.Application\Albingia.Kheops.OP.Domain",
            @"OP.Application\Albingia.Kheops.OP.DataAdapter",
            @"OP.Application\Albingia.Kheops.OP.Application",
            @"ServiceMetierOP\OP.IOWebService",
            @"ServiceMetierOP\OP.Services",
            @"ServiceMetierOP\OPServiceContract",
            @"Port\Driver",
            @"Port\Driven",
            @"Infrastructure\Services",
            @"Configs\Dev"
        };
        static string rootPath = @"C:\tfs\neos5\Kheops\Support\Kheops";
        static string DomainPath => pathList.First();
        static string DataAdapterPath => pathList[1];
        static string ApplicationPath => pathList[2];
        static string PortSubPath => pathList[6];
        static string DrivenSubPath => pathList[7];
        static string ServiceSubPath => pathList[8];
        static string WCFPath => pathList[3];
        static string WCFServicesPath => pathList[4];
        static string WCFContractsPath => pathList[5];
        static string WCFConfigPath => pathList.Last();

        static void Main(string[] args) {
            try {
                rootPath = "../../..";
                if (new DirectoryInfo(rootPath).Name != "Kheops") {
                    Console.WriteLine("Folder root must be Kheops");
                    return;
                }
                CheckArguments(args);
                IncludeSvc(args[0]);
                IncludeServiceContract(args[0]);
                IncludeService(args[0]);
                IncludeDomainClass(args[0]);
                IncludePortDriver(args[0]);
                IncludePortDriven(args[0]);
                IncludeRepository(args[0]);
            }
            catch (Exception ex) {
                Console.WriteLine("ERROR ---------------------------------");
                Console.WriteLine();
                Console.WriteLine(ex);
                Console.WriteLine();
                Console.WriteLine("---------------------------------");
            }
        }

        private static void IncludeSvc(string label) {
            string projectPath = Path.Combine(rootPath, WCFPath);
            string svcName = $"{label}{LabelSuffix.Service}{svcExt}";
            string filename = Path.Combine(projectPath, svcName);
            string projectFile = Path.Combine(projectPath, WCFPath.Split('\\').Last() + projectExt);
            var xmlDoc = XDocument.Load(projectFile);
            var parent = GetContentItemGroup(xmlDoc.Root);

            AddFileToElement(ref parent, svcName, CsProjKeyword.Content);

            using (var writer = File.CreateText(filename)) {
                writer.WriteLine($"<%@ ServiceHost Language=\"C#\" Debug=\"true\" Service=\"{WCFContractsPath.Split('\\').Last()}.{label}{LabelSuffix.Service}\" Factory=\"LightInject.WcfCustom.LightInjectServiceHostFactory\" %>");
                writer.Flush();
            }

            xmlDoc.Save(projectFile);

            // set endpoints
            filename = Path.Combine(projectPath, WCFConfigPath, "system.serviceModel.services.xml");
            xmlDoc = XDocument.Load(filename);
            var serviceNode = xmlDoc.Root.Elements(WCFKeyword.service.ToString())
                .FirstOrDefault(e => e.Attribute(WCFKeyword.behaviorConfiguration.ToString())?.Value == "OffreServiceBehavior");
            if (serviceNode is null) {
                return;
            }
            string serviceNodeName = $"{WCFServicesPath.Split('\\').Last()}.{label}{LabelSuffix.Service}";
            if (xmlDoc.Root.Elements(WCFKeyword.service.ToString()).Any(e => e.Attribute(WCFKeyword.name.ToString())?.Value == serviceNodeName)) {
                return;
            }
            var newServiceNode = new XElement(serviceNode);
            newServiceNode.Attribute(WCFKeyword.name.ToString()).Value = serviceNodeName;

            var endpointNode = newServiceNode.Elements(WCFKeyword.endpoint.ToString())
                .First(e => e.Attribute(WCFKeyword.bindingConfiguration.ToString())?.Value == "KheopsBasicHttpBinding");
            var urlSplit = endpointNode.Attribute(WCFKeyword.address.ToString()).Value.Split('/').ToList();
            while (urlSplit.Count > 3) urlSplit.RemoveAt(urlSplit.Count - 1);
            urlSplit.Add(svcName);
            endpointNode.Attribute(WCFKeyword.address.ToString()).Value = string.Join("/", urlSplit);
            endpointNode.Attribute(WCFKeyword.contract.ToString()).Value = $"{WCFContractsPath.Split('\\').Last()}.I{label}";

            xmlDoc.Root.Add(newServiceNode);

            // let file as an xml fragment
            File.WriteAllText(filename, xmlDoc.ToString(), Encoding.UTF8);
        }

        private static void IncludeServiceContract(string label) {
            string projectPath = Path.Combine(rootPath, WCFContractsPath);
            string filename = Path.Combine(projectPath, $"I{label}{csharpExt}");
            string csns = WCFContractsPath.Split('\\').Last();
            string projectFile = Path.Combine(projectPath, csns + projectExt);
            var xmlDoc = XDocument.Load(projectFile);
            var parent = GetCompileItemGroup(xmlDoc.Root);

            AddFileToElement(ref parent, $"I{label}{csharpExt}");

            using (var writer = File.CreateText(filename)) {
                writer.WriteLine($@"
using System.ServiceModel;

namespace {csns} {{
    [ServiceContract]
    public interface I{label} {{
    }}
}}
");
                writer.Flush();
            }

            xmlDoc.Save(projectFile);
        }

        private static void IncludeService(string label) {
            string projectPath = Path.Combine(rootPath, WCFServicesPath);
            string filename = Path.Combine(projectPath, $"{label}{LabelSuffix.Service}{csharpExt}");
            string csns = WCFServicesPath.Split('\\').Last();
            string projectFile = Path.Combine(projectPath, csns + projectExt);
            var xmlDoc = XDocument.Load(projectFile);
            var parent = GetCompileItemGroup(xmlDoc.Root);

            AddFileToElement(ref parent, $"{label}{LabelSuffix.Service}{csharpExt}");

            using (var writer = File.CreateText(filename)) {
                writer.WriteLine($@"
using {string.Join(".", new[] { ApplicationPath.Split('\\').Last() }.Concat(PortSubPath.Split('\\')))};
using {WCFContractsPath.Split('\\').Last()};
using System.ServiceModel.Activation;

namespace {csns} {{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class {label}{LabelSuffix.Service} : I{label} {{
        private readonly I{label}{LabelSuffix.Port} {LabelSuffix.Service.ToString().ToLower()};
        public {label}{LabelSuffix.Service}(I{label}{LabelSuffix.Port} {LabelSuffix.Service.ToString().ToLower()}) {{
            this.{LabelSuffix.Service.ToString().ToLower()} = {LabelSuffix.Service.ToString().ToLower()};
        }}
    }}
}}
");
                writer.Flush();
            }

            xmlDoc.Save(projectFile);
        }

        private static void IncludeDomainClass(string label) {
            
        }

        private static void IncludePortDriver(string label) {
            string projectPath = Path.Combine(rootPath, ApplicationPath);
            string filename = Path.Combine(projectPath, PortSubPath, $"I{label}{LabelSuffix.Port}{csharpExt}");
            string csns = string.Join(".", new[] { ApplicationPath.Split('\\').Last() }.Concat(PortSubPath.Split('\\')));
            string projectFile = Path.Combine(projectPath, ApplicationPath.Split('\\').Last() + projectExt);
            var xmlDoc = XDocument.Load(projectFile);
            var parent = GetCompileItemGroup(xmlDoc.Root);

            AddFileToElement(ref parent, $"{PortSubPath}\\I{label}{LabelSuffix.Port}{csharpExt}");

            using (var writer = File.CreateText(filename)) {
                writer.WriteLine($@"
using System.ServiceModel;

namespace {csns} {{
    [ServiceContract]
    public interface I{label}{LabelSuffix.Port} {{
    }}
}}
");
                writer.Flush();
            }

            filename = Path.Combine(projectPath, ServiceSubPath, $"{label}{LabelSuffix.Service}{csharpExt}");
            csns = string.Join(".", new[] { ApplicationPath.Split('\\').Last() }.Concat(ServiceSubPath.Split('\\')));
            AddFileToElement(ref parent, $"{ServiceSubPath}\\{label}{LabelSuffix.Service}{csharpExt}");

            using (var writer = File.CreateText(filename)) {
                writer.WriteLine($@"
using {string.Join(".", new[] { ApplicationPath.Split('\\').Last() }.Concat(DrivenSubPath.Split('\\')))};
using {string.Join(".", new[] { ApplicationPath.Split('\\').Last() }.Concat(PortSubPath.Split('\\')))};

namespace {csns} {{
    public class {label}{LabelSuffix.Service} : I{label}{LabelSuffix.Port} {{
        private readonly I{label}{LabelSuffix.Repository} {LabelSuffix.Repository.ToString().ToLower()};
        public {label}{LabelSuffix.Service}(I{label}{LabelSuffix.Repository} {LabelSuffix.Repository.ToString().ToLower()}) {{
            this.{LabelSuffix.Repository.ToString().ToLower()} = {LabelSuffix.Repository.ToString().ToLower()};
        }}
    }}
}}
");
                writer.Flush();
            }

            xmlDoc.Save(projectFile);
        }

        private static void IncludePortDriven(string label) {
            string projectPath = Path.Combine(rootPath, ApplicationPath);
            string filename = Path.Combine(projectPath, DrivenSubPath, $"I{label}{LabelSuffix.Repository}{csharpExt}");
            string csns = string.Join(".", new[] { ApplicationPath.Split('\\').Last() }.Concat(DrivenSubPath.Split('\\')));
            string projectFile = Path.Combine(projectPath, ApplicationPath.Split('\\').Last() + projectExt);
            var xmlDoc = XDocument.Load(projectFile);
            var parent = GetCompileItemGroup(xmlDoc.Root);

            AddFileToElement(ref parent, $"{DrivenSubPath}\\I{label}{LabelSuffix.Repository}{csharpExt}");

            using (var writer = File.CreateText(filename)) {
                writer.WriteLine($@"
namespace {csns} {{
    public interface I{label}{LabelSuffix.Repository} {{
    }}
}}
");
                writer.Flush();
            }

            xmlDoc.Save(projectFile);
        }

        private static void IncludeRepository(string label) {
            string projectPath = Path.Combine(rootPath, DataAdapterPath);
            string filename = Path.Combine(projectPath, $"{label}{LabelSuffix.Repository}{csharpExt}");
            string csns = DataAdapterPath.Split('\\').Last();
            string projectFile = Path.Combine(projectPath, csns + projectExt);
            var xmlDoc = XDocument.Load(projectFile);
            var parent = GetCompileItemGroup(xmlDoc.Root);

            AddFileToElement(ref parent, $"{label}{LabelSuffix.Repository}{csharpExt}");

            using (var writer = File.CreateText(filename)) {
                writer.WriteLine($@"
using {string.Join(".", new[] { ApplicationPath.Split('\\').Last() }.Concat(DrivenSubPath.Split('\\')))};

namespace {csns} {{
    public class {label}{LabelSuffix.Repository} : I{label}{LabelSuffix.Repository} {{
    }}
}}
");
                writer.Flush();
            }

            xmlDoc.Save(projectFile);
        }

        private static void CheckArguments(string[] args) {
            if (!Regex.IsMatch(args?.FirstOrDefault() ?? string.Empty, @"^[a-z_A-Z]\w{0,127}$", RegexOptions.Singleline)) {
                throw new ArgumentException(nameof(args));
            }
        }

        static XElement GetContentItemGroup(XElement root) {
            return root.Elements(XName.Get(CsProjKeyword.ItemGroup.ToString(), "http://schemas.microsoft.com/developer/msbuild/2003")).FirstOrDefault(e =>
                e.Elements(XName.Get(CsProjKeyword.Content.ToString(), "http://schemas.microsoft.com/developer/msbuild/2003"))
                    .Any(c => c.Attribute(CsProjKeyword.Include.ToString())?.Value.EndsWith(csharpExt) ?? false));
        }

        static XElement GetCompileItemGroup(XElement root) {
            return root.Elements(XName.Get(CsProjKeyword.ItemGroup.ToString(), "http://schemas.microsoft.com/developer/msbuild/2003")).FirstOrDefault(e =>
                e.Elements(XName.Get(CsProjKeyword.Compile.ToString(), "http://schemas.microsoft.com/developer/msbuild/2003"))
                    .Any(c => c.Attribute(CsProjKeyword.Include.ToString())?.Value.EndsWith(csharpExt) ?? false));
        }

        static void AddFileToElement(ref XElement element, string name, CsProjKeyword csprojKeyword = CsProjKeyword.Compile) {
            var xname = XName.Get(csprojKeyword.ToString(), "http://schemas.microsoft.com/developer/msbuild/2003");
            if (element.Elements(xname).Any(e => e.Attribute(CsProjKeyword.Include.ToString())?.Value == name)) {
                return;
            }
            element.Add(new XElement(xname, new XAttribute(CsProjKeyword.Include.ToString(), name)));
        }
    }
}
