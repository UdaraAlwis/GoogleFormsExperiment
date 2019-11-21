using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleFormsExperiment.Models
{
    public class GoogleFormField
    {
        /// <summary>
        /// Type of Question Field
        /// </summary>
        public GoogleFormsFieldTypeEnum Type { get; set; }

        /// <summary>
        /// Question text of the field
        /// </summary>
        public string QuestionString { get; set; }

        /// <summary>
        /// The Id need to be used 
        /// when submitting the answer
        /// </summary>
        public string SubmissionId { get; set; }

        /// <summary>
        /// Answer List in any type of 
        /// multiple answer selection field
        /// </summary>
        public List<string> AnswerList { get; set; } = new List<string>();

        /// <summary>
        /// User's answer to the question field
        /// </summary>
        public string UserAnswer { get; set; }
    }
}
