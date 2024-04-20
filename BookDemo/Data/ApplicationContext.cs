﻿using BookDemo.Models;

namespace BookDemo.Data
{
    public static class ApplicationContext
    {
        public static List<Book> Books { get; set; }
        static ApplicationContext()
        {
            Books = new List<Book>()
            {
                new Book(){Id = 1, Title ="Hacivat", Price = 75},
                new Book(){Id = 2, Title="Mesnevi", Price= 100},
                new Book(){Id = 3, Title="Dede Korkut", Price= 125}
            };
        }
    }
}
