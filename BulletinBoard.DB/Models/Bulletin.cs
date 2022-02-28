using System;

namespace BulletinBoard.DB.Models
{
    public class Bulletin
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string PhotoLinks { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
