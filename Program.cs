using System.Text.Json;

namespace BunkBedBomRouting
{
    class BomItem
    {
        public string description { get; set; } = "";
        public int quantity { get; set; }
        public int step { get; set; } = -1;
        public string source { get; set; } = "";
        public BomItem[] bom { get; set; } = Array.Empty<BomItem>();
    }

    class RoutingItem
    {
        public int step { get; set; }
        public string description { get; set; } = "";
        public int taktTime { get; set; }
    }

    class Program
    {
        /// <summary>
        /// Gets all components and subcomponents of the given BOM.
        /// </summary>
        /// <param name="bom">BOM object type</param>
        /// <returns>Array of all components and subcomponents.</returns>
        static BomItem[] GetAllComponents(BomItem bom)
        {
            List<BomItem> list = new List<BomItem>();
            foreach (BomItem item in bom.bom)
            {
                list.Add(item);
                list.AddRange(GetAllComponents(item));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Gets all components that contain unprovided subcomponents in its BOM.
        /// </summary>
        /// <param name="bom">BOM object type</param>
        /// <returns>Array of components with unprovided materials.</returns>
        static BomItem[] GetStepsWithUnprovidedComponents(BomItem bom)
        {
            List<BomItem> list = new List<BomItem>();
            foreach (BomItem item in bom.bom)
            {
                // if the component has a BOM and at least one of its subcomponents is not provided
                if (item.bom.Length > 0 && item.bom.Where(x => !x.source.ToLower().Equals("provided")).Any())
                {
                    list.Add(item);
                    list.AddRange(GetStepsWithUnprovidedComponents(item));
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Sums the quantities of all components in the given BOM and writes the results to a file.
        /// </summary>
        /// <param name="bom">BOM object type</param>
        static BomItem[] GetSumQuantities(BomItem bom)
        {
            List<BomItem> componentSums = new List<BomItem>();
            BomItem[] allComponents = GetAllComponents(bom).ToArray();
            foreach (BomItem bomItem in allComponents)
            {
                int foundIndex = componentSums.FindIndex(x => x.description == bomItem.description);
                if (foundIndex > -1)
                {
                    componentSums[foundIndex].quantity += bomItem.quantity;
                }
                else
                {
                    componentSums.Add(bomItem);
                }
            }
            return componentSums.ToArray();
        }

        /// <summary>
        /// Writes all total component summed quantities to the file 'output.csv'.
        /// </summary>
        /// <param name="bom">BOM object type</param>
        static void WriteSumOutput(BomItem bom)
        {
            BomItem[] providedComponents = GetSumQuantities(bom).Where(x => x.source.ToLower().Equals("provided")).ToArray();
            string fileContent = "component,quantity\n";
            foreach (BomItem component in providedComponents)
            {
                fileContent += string.Format("{0},{1}\n", component.description, component.quantity);
            }
            File.WriteAllText("output.csv", fileContent);
        }

        /// <summary>
        /// Prints all components with unprovided subcomponents to console.
        /// </summary>
        /// <param name="bom">BOM object type</param>
        static void UnprovidedComponentSteps(BomItem bom)
        {
            BomItem[] unprovidedSteps = GetStepsWithUnprovidedComponents(bom).ToArray();
            foreach (BomItem component in unprovidedSteps)
            {
                Console.WriteLine("Step {0} '{1}' has no provided components added.", component.step, component.description);
            }
        }

        /// <summary>
        /// Calculates and prints the total takt time of the given BOM based on its provided routings.
        /// </summary>
        /// <param name="bom">BOM object type</param>
        /// <param name="routings">RoutingItem array</param>
        static void CalculateTaktTime(BomItem bom, RoutingItem[] routings)
        {
            BomItem[] allComponents = GetAllComponents(bom);
            RoutingItem[] componentRoutings = routings.Where(x => allComponents.Any(o => o.step == x.step)).ToArray(); // filter out routings not apart of the bom
            int totalSeconds = componentRoutings.Sum(x => x.taktTime);
            double totalMinutes = totalSeconds / 60.0;
            Console.WriteLine("Overall takt time: {0} minutes", string.Format("{0:0.00}", totalMinutes));
        }

        static void ProgramEnd(string message)
        {
            Console.WriteLine("\n" + message + "\n");
            Console.WriteLine("Press any key to exit:");
            Console.ReadKey();
        }

        static void Main()
        {
            try
            {
                FileStream bomFile = File.OpenRead("bunkbed-bom.json");
                FileStream routingFile = File.OpenRead("bunkbed-routing.json");
                BomItem bom = JsonSerializer.Deserialize<BomItem>(bomFile) ?? throw new Exception("Failed to parse 'bunkbed-bom.json'.");
                RoutingItem[] routings = JsonSerializer.Deserialize<RoutingItem[]>(routingFile) ?? throw new Exception("Failed to parse 'bunkbed-routing.json'.");
                WriteSumOutput(bom);
                UnprovidedComponentSteps(bom);
                CalculateTaktTime(bom, routings);
                ProgramEnd("Bunk Bed Build Takt Time Calculator finished successfully.");
            }
            catch (Exception e)
            {
                ProgramEnd("Error running Bunk Bed Build Takt Time Calculator.\n" + e.Message);
            }
        }
    }
}
