using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleFormsExperiment.Models
{
    /// <summary>
    /// A Question Field in a Google Form
    /// </summary>
    public class GoogleFormField
    {
        /// <summary>
        /// Type of the Question Field
        /// </summary>
        public GoogleFormsFieldTypeEnum Type { get; set; }

        /// <summary>
        /// Question text of the Field
        /// </summary>
        public string QuestionString { get; set; }

        /// <summary>
        /// The unique Id need to be used 
        /// when submitting the answer
        /// I also refer to this as: Field Id
        /// </summary>
        public string SubmissionId { get; set; }

        /// <summary>
        /// Available Answer List for any kind of 
        /// multiple answer selection field
        /// </summary>
        public List<string> AnswerList { get; set; } = new List<string>();

        /// <summary>
        /// If the answer is required to Submit
        /// </summary>
        public bool IsAnswerRequired { get; set; }
    }
}
