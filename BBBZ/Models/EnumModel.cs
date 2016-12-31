﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBBZ.Models
{
    public enum CategorysTemplate
    {
        NotSet = 0,
        
        Default = 1,
        LessonOneByOne = 4,
        AsLinks = 2,

        News,

        QuestionBank,

        ArticleList = 11,

        // add new item at last of the list, and don't change the numbers or names
    }

    public enum ContentsTemplate
    {
        NotSet = 0,

        Article = 1,
        JustTheContent = 2,

        Lesson = 3,

        Files = 7,
        Video = 8,



        OptionalQuiz = 4,
        MandatoryQuiz = 5,

        Exam = 6,



        // add new item at last of the list, and don't change the numbers or names
    }
}