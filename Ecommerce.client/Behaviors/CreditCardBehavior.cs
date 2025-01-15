//using Microsoft.Maui.Controls;
//using System.Text.RegularExpressions;

//namespace Ecommerce.client.Behaviors
//{
//    public class CreditCardBehavior : Behavior<Entry>
//    {
//        protected override void OnAttachedTo(Entry bindable)
//        {
//            bindable.MaxLength = 19; // 16 digits + 3 spaces
//            bindable.TextChanged += OnTextChanged;
//            bindable.Unfocused += OnUnfocused;
//            base.OnAttachedTo(bindable);
//        }

//        protected override void OnDetachingFrom(Entry bindable)
//        {
//            bindable.TextChanged -= OnTextChanged;
//            bindable.Unfocused -= OnUnfocused;
//            base.OnDetachingFrom(bindable);
//        }

//        private void OnTextChanged(object sender, TextChangedEventArgs e)
//        {
//            var entry = sender as Entry;
//            if (entry == null) return;

//            // Remove all non-digit characters
//            string digits = Regex.Replace(entry.Text, @"[^\d]", "");

//            // Insert space every 4 digits
//            string formatted = "";
//            for (int i = 0; i < digits.Length; i++)
//            {
//                if (i > 0 && i % 4 == 0)
//                    formatted += " ";
//                formatted += digits[i];
//            }

//            // Prevent infinite loop
//            if (formatted != entry.Text)
//            {
//                int cursorPosition = entry.CursorPosition;
//                entry.Text = formatted;
//                entry.CursorPosition = Math.Min(cursorPosition, formatted.Length);
//            }
//        }

//        private void OnUnfocused(object sender, FocusEventArgs e)
//        {
//            var entry = sender as Entry;
//            if (entry == null) return;

//            // Remove spaces before sending data
//            entry.Text = entry.Text.Replace(" ", "");
//        }
//    }
//}






//---------------------------------> not implemented yet 