using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleFormsExperiment.Models
{
    public class GoogleFormField
    {
        public GoogleFormsFieldTypeEnum Type { get; set; }

        public string QuestionString { get; set; }

        public string SubmissionId { get; set; }
    }
}
