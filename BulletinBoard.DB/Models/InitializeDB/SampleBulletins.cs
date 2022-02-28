using System;
using System.Collections.Generic;
using System.Linq;

namespace BulletinBoard.DB.Models.InitializeDB
{
    public static class SampleBulletins
    {
        public static void Initialize(BulletinsDBContext context)
        {
            if (!context.Bulletins.Any())
            {
                context.Bulletins.AddRange(
                    new Bulletin
                    {
                        ID = Guid.NewGuid(),
                        Name = "IPhone 12",
                        Description = "Продам айфон, состояние нового, пользовались 2 недели" +
                        ", потом надоел, купили Nokia 3310, все вопросы по телефону +375251112233",
                        PhotoLinks = "photo1.jpg, photo2.jpg, photo3.jpg",
                        Price = 1599.99,
                        CreationDate = DateTime.Now
                    },
                    new Bulletin
                    {
                        ID = Guid.NewGuid(),
                        Name = "Xiaomi MI 12 Pro",
                        Description = "Продам Xiaomi MI 12 Pro, состояние 8/10, пользовались 1 год" +
                        ", продаём по причине покупки IPhone 12, все вопросы по телефону +375293332211",
                        PhotoLinks = "xi1.jpg, xi2.jpg" ,
                        Price = 899.99,
                        CreationDate = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
