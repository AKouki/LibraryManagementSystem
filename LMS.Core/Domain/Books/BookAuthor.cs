﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core.Domain.Books
{
    public class BookAuthor
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
