namespace EmployeeTraingsTracker.Model
{
    public class EmployeeTraining
    {
        public int EmployeeId { get; set; }  // Foreign Key
        public Employee Employee { get; set; }  // Navigation property

        public int TrainingId { get; set; }  // Foreign Key
        public Training Training { get; set; }  // Navigation property

        public DateTime CompletedOn { get; set; } // optional extra info
    }
}
