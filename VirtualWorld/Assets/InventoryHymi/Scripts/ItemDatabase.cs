using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//namespace Hymi
//{
//    public class ItemDatabase : MonoBehaviour
//    {
        //public List<Item> items = new List<Item>();

        //private void Awake()
        //{
        //    BuildDatabase();
        //}

        ////Hakee itemin ID:n mukaan
        //public Item GetItem(int id)
        //{
        //    return items.Find(item => item.id == id);
        //}

        ////Hakee itemin nimen mukaan
        //public Item GetItem(string name)
        //{
        //    return items.Find(item => item.name == name);
        //}

        /*
        Täsä voidaan lisätä uusia itemeitä. Item(id, name, type, stackable, description, stats)
        Type = Food, Drink, Key, Ticket, Token, Guidebook, Tool, Equipment
        Stats = Value, Hunger, Thirst

        ID:t:
        0-99 = Food
        100-199 = Drink
        200-299 = Key
        300-399 = Ticket
        400-499 = Token
        500-599 = Guidebook
        600-699 = Tool
        700-799 = Equipment

        Kuva tulee laittamalla kuva kansioon Resources/Sprites, ja nimeämällä sen saman nimiseksi kuin item.
        */
//        void BuildDatabase()
//        {
//            items = new List<Item> {
//            new Item(0, "Sandwich", "Food", true, "Tasty sandwich with various fillings.",
//            new Dictionary<string, double>{
//                {"Value", 5.0},
//                {"Hunger", -5.0}
//            }),

//            new Item(1, "Chips Bag", "Food", true, "A bag of chips.",
//            new Dictionary<string, double>{
//                {"Value", 4.0}
//            }),

//            new Item(100, "Bottled Water", "Drink", true, "A refreshing bottle of water.",
//            new Dictionary<string, double>{
//                {"Value", 2.0},
//                {"Thirst", -5.0}
//            }),

//            new Item(101, "Soft Drink", "Drink", true, "A bottle of soft drink.",
//            new Dictionary<string, double>{
//                {"Value", 4.0}
//            }),

//            new Item(200, "Hotel Room Key", "Key", false, "The key to your hotel room.",
//            new Dictionary<string, double>{
//                {"Value", 50.0}
//            }),

//            new Item(300, "Museum Ticket", "Ticket", false, "Ticket for entry to the museum.",
//            new Dictionary<string, double>{
//                {"Value", 15.0}
//            }),

//            new Item(301, "Art Gallery Ticket", "Ticket", false, "Ticket for entry to the art gallery.",
//            new Dictionary<string, double>{
//                {"Value", 20.0}
//            }),

//            new Item(302, "Theme Park Pass", "Ticket", false, "All-day pass to the theme park.",
//            new Dictionary<string, double>{
//                {"Value", 30.0}
//            }),

//            new Item(303, "Space Elevator Ticket", "Ticket", false, "Ticket to ride the space elevator.",
//            new Dictionary<string, double>{
//                {"Value", 50.0}
//            }),

//            new Item(400, "Arcade Game Token", "Token", true, "Single token for arcade games.",
//            new Dictionary<string, double>{
//                {"Value", 3.0}
//            }),

//            new Item(401, "Arcade Token Bundle", "Token", true, "Bundle of tokens for the arcade.",
//            new Dictionary<string, double>{
//                {"Value", 25.0}
//            }),

//            new Item(500, "Museum Guidebookr", "Guidebook", false, "Guide to the museum's collections.",
//            new Dictionary<string, double>{
//                {"Value", 8.0}
//            }),

//            new Item(501, "Art Gallery Guidebook", "Guidebook", false, "Guidebook for art enthusiasts.",
//            new Dictionary<string, double>{
//                {"Value", 8.0}
//            }),

//            new Item(502, "City Tour Guidebook", "Guidebook", false, "Guide to city landmarks.",
//            new Dictionary<string, double>{
//                {"Value", 10.0}
//            }),

//            new Item(601, "Hammer", "Tool", false, "Durable hammer for repair and maintenance.",
//            new Dictionary<string, double>{
//                {"Value", 20.0}
//            }),

//            new Item(602, "Wrench", "Tool", false, "Versatile wrench for various uses.",
//            new Dictionary<string, double>{
//                {"Value", 20.0}
//            }),

//            new Item(700, "Screwdriver Set", "Tool", false, "Set of screwdrivers.",
//            new Dictionary<string, double>{
//                {"Value", 15.0}
//            }),

//            new Item(701, "Tape Measure", "Tool", false, "Retractable tape measure.",
//            new Dictionary<string, double>{
//                {"Value", 10.0}
//            }),

//            new Item(702, "Power Drill", "Tool", false, "Powerful electric drill.",
//            new Dictionary<string, double>{
//                {"Value", 50.0}
//            }),

//            new Item(703, "Soldering Iron", "Tool", false, "Tool to melt solder.",
//            new Dictionary<string, double>{
//                {"Value", 25.0}
//            }),

//            new Item(800, "Casual Clothing Set", "Equipment", false, "Everyday casual wear.",
//            new Dictionary<string, double>{
//                {"Value", 30.0}
//            }),

//            new Item(801, "Formal Suit", "Equipment", false, "Stylish formal suit.",
//            new Dictionary<string, double>{
//                {"Value", 60.0}
//            }),

//            new Item(802, "Workwear Set", "Equipment", false, "Clothing for tough conditions.",
//            new Dictionary<string, double>{
//                {"Value", 40.0}
//            }),

//            new Item(803, "Safety Helmet", "Equipment", false, "Protective construction helmet.",
//            new Dictionary<string, double>{
//                {"Value", 20.0}
//            }),

//            new Item(804, "Kitchen Apron", "Equipment", false, "Apron for cooks and waiters.",
//            new Dictionary<string, double>{
//                {"Value", 10.0}
//            }),
//        };
//        }
//    }
//}
