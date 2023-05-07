using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public TodoItem(string text)
        {
            Text = text;
        }

        public long Id { get; set; }
        [DataType(DataType.Text)] public string Text { get; set; }
        public ItemStatus Status { get; set; }

        public enum ItemStatus
        {
            Todo,
            Doing,
            Done
        }
    }
}