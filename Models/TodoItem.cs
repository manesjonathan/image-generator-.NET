using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        [DataType(DataType.Text)] public string Text { get; set; }
        public Status ItemStatus { get; set; }

        public enum Status
        {
            todo,
            doing,
            done
        }
    }
}