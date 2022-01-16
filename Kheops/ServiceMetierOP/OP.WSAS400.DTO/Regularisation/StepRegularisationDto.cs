using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class StepRegularisationDto
    {
        private RegularisationStep? step;

        public StepRegularisationDto()
        {
            Message = string.Empty;
            Step = null;
            ErrorIfMessage = true;
        }

        public RegularisationStep? Step
        {
            get
            {
                if (ErrorIfMessage)
                {
                    if (string.IsNullOrWhiteSpace(Message))
                    {
                        return step;
                    }
                    else
                    {
                        return RegularisationStep.Invalid;
                    }
                }

                return step;
            }
            set
            {
                if (ErrorIfMessage)
                {
                    if (string.IsNullOrWhiteSpace(Message))
                    {
                        step = value;
                    }
                }
                else
                {
                    step = value;
                }
            }
        }

        public string Message { get; set; }

        public bool ErrorIfMessage { get; set; }
    }
}
