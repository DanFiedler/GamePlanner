using System.Xml.Serialization;
using System.Collections.Generic;

namespace GamePlannerWeb.BGGClient.BggCollection
{
    [XmlRoot(ElementName = "name")]
    public class Name
    {
        [XmlAttribute(AttributeName = "sortindex")]
        public string Sortindex { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "status")]
    public class Status
    {
        [XmlAttribute(AttributeName = "own")]
        public string Own { get; set; }
        [XmlAttribute(AttributeName = "prevowned")]
        public string Prevowned { get; set; }
        [XmlAttribute(AttributeName = "fortrade")]
        public string Fortrade { get; set; }
        [XmlAttribute(AttributeName = "want")]
        public string Want { get; set; }
        [XmlAttribute(AttributeName = "wanttoplay")]
        public string Wanttoplay { get; set; }
        [XmlAttribute(AttributeName = "wanttobuy")]
        public string Wanttobuy { get; set; }
        [XmlAttribute(AttributeName = "wishlist")]
        public string Wishlist { get; set; }
        [XmlAttribute(AttributeName = "preordered")]
        public string Preordered { get; set; }
        [XmlAttribute(AttributeName = "lastmodified")]
        public string Lastmodified { get; set; }
    }

    [XmlRoot(ElementName = "item")]
    public class Item
    {
        [XmlElement(ElementName = "name")]
        public Name Name { get; set; }
        [XmlElement(ElementName = "yearpublished")]
        public string Yearpublished { get; set; }
        [XmlElement(ElementName = "image")]
        public string Image { get; set; }
        [XmlElement(ElementName = "thumbnail")]
        public string Thumbnail { get; set; }
        [XmlElement(ElementName = "status")]
        public Status Status { get; set; }
        [XmlElement(ElementName = "numplays")]
        public string Numplays { get; set; }
        [XmlAttribute(AttributeName = "objecttype")]
        public string Objecttype { get; set; }
        [XmlAttribute(AttributeName = "objectid")]
        public string Objectid { get; set; }
        [XmlAttribute(AttributeName = "subtype")]
        public string Subtype { get; set; }
        [XmlAttribute(AttributeName = "collid")]
        public string Collid { get; set; }
    }

    [XmlRoot(ElementName = "items")]
    public class Items
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
        [XmlAttribute(AttributeName = "totalitems")]
        public string Totalitems { get; set; }
        [XmlAttribute(AttributeName = "termsofuse")]
        public string Termsofuse { get; set; }
        [XmlAttribute(AttributeName = "pubdate")]
        public string Pubdate { get; set; }
    }

}