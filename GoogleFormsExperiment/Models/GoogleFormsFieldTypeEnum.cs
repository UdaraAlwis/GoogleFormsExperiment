using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleFormsExperiment.Models
{
    /// <summary>
    /// Found the Field type representation values with trial
    /// and error try out of blood sweat and tears lol! ;)
    /// </summary>
    public enum GoogleFormsFieldTypeEnum
    {
        ShortAnswerField = 0,
        ParagraphField = 1,

        MultipleChoiceField = 2,
        CheckBoxesField = 4,
        DropDownField = 3,

        // FileUpload - Not supported (needs user log in session)
        FileUploadField = 13, 

        LinearScaleField = 5,
        // represents both: Multiple Choice Grid | Checkbox Grid
        GridChoiceField = 7, 

        DateField = 9,
        TimeField = 10,
    }
}
