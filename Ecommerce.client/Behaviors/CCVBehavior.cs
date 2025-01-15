//using Microsoft.Maui.Controls;
//using System.Text.RegularExpressions;

//namespace Ecommerce.client.Behaviors
//{
//    public class CCVBehavior : Behavior<Entry>
//    {
//        protected override void OnAttachedTo(Entry bindable)
//        {
//            bindable.MaxLength = 4;
//            bindable.IsPassword = true; // Masks the input
//            bindable.Keyboard = Keyboard.Numeric;
//            bindable.TextChanged += OnTextChanged;
//            base.OnAttachedTo(bindable);
//        }

//        protected override void OnDetachingFrom(Entry bindable)
//        {
//            bindable.TextChanged -= OnTextChanged;
//            base.OnDetachingFrom(bindable);
//        }

//        private void OnTextChanged(object sender, TextChangedEventArgs e)
//        {
//            var entry = sender as Entry;
//            if (entry == null) return;

//            // Ensure only digits are entered
//            string digits = Regex.Replace(entry.Text, @"[^\d]", "");

//            if (digits != entry.Text)
//            {
//                entry.Text = digits;
//            }
//        }
//    }
//}






//-------------------------> not implemented yet 