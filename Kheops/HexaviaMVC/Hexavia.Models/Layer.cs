
namespace Hexavia.Models
{
    /// <summary>
    /// Layer structure
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// Unique uid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// layer name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// layer caracteristique
        /// </summary>
        public string Caracteristique { get; set; }

        /// <summary>
        /// layer Branche
        /// </summary>
        public string Branche { get; set; }

        /// <summary>
        /// layer modifiable
        /// </summary>
        public string Modifiable { get; set; }

        /// <summary>
        /// Geojson as string
        /// </summary>
        public string Shape { get; set; }

        /// <summary>
        /// Geojson as string
        /// </summary>
        public string Date { get; set; }
    }
}
