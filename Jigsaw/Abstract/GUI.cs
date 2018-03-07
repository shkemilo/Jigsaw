using System.Windows.Forms;

namespace Jigsaw
{
     /// <summary>
     /// Superclass for GUI elements.
     /// </summary>
    public abstract class GUIElement : Observer
    {
        /// <summary> Shows the GUI element's message to display. </summary>
        public abstract void Show();

        /// <summary> Updates the GUI element of a change of it's message to display. </summary>
        public abstract void Update<T>(T message);
    }

     /// <summary>
     /// Extension for Animated GUI elements.
     /// </summary>
    public interface Animateable
    {
        /// <summary> Animates the GUI element. </summary>
        void Animate();
    }

    /// <summary>
    /// Simple display used for displaying a message.
    /// </summary>
    public class Display : GUIElement
    {
        Control messageDisplay;

        string message;

        public Display(Control messageDisplay)
        {
            message = "";
            this.messageDisplay = messageDisplay;
        }

        /// <summary> Updates the message to be shown. </summary>
        public override void Update<T>(T message)
        {
            this.message = message.ToString();
        }

        /// <summary> Shows the current message. </summary>
        public override void Show()
        {
            messageDisplay.Text = message.ToString();
        }
    }

    
}
