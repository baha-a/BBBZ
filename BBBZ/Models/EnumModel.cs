using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBBZ.Models
{
    public enum CategorysTemplate
    {
        NotSet = 0,

        Course,
        ArticleList,

        LessonOneByOne,
        LessonRandom,

        QuestionBank,

        Custom
    }

    public enum ContentsTemplate
    {
        NotSet = 0,

        Article,

        Lesson,
        OptionalQuiz,
        MandatoryQuiz,

        Exam,

        File,

        Custom
    }
}