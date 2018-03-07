using System;
using System.Windows.Forms;
using System.Drawing;
using MetroFramework.Controls;

namespace Jigsaw.Jumper
{
    /// <summary>
    /// GUIEelement used to represent one field for displaying one element of the current answer in the Jumper Game.
    /// </summary>
    public class JumperDisplayElement : GUIElement
    {
        Control field;
        Image elementImage;

        public JumperDisplayElement(Control field)
        {
            this.field = field;

            elementImage = null;
        }

        /// <summary> Changes the controls enabled state. </summary>
        public void SetEnabled(bool b)
        {
            field.Enabled = b;
        }

        /// <summary> Draws this element. </summary>
        public override void Show()
        {
            if (elementImage != null)
                (field as MetroTile).TileImage = new Bitmap(elementImage);
            else
                (field as MetroTile).TileImage = null;

            field.Refresh();
        }

        /// <summary> Updates the image the element will show. </summary>
        public override void Update<T>(T message)
        {
            if (message == null)
                elementImage = null;
            else if (message is Image)
                elementImage = message as Image;
            else
                throw new Exception("This function only accepts Images");
        }
    }

    /// <summary>
    /// GUIElement used to represent current answer in the Jumper Game.
    /// </summary>
    public class JumperDisplayComponent : GUIElement
    {
        JumperDisplayElement[] elements;

        int numberOfElements;
        int currentElement;

        public JumperDisplayComponent(JumperDisplayElement[] elements, int numberOfElements = 4)
        {
            this.elements = elements;

            this.numberOfElements = numberOfElements;
        }

        public int CurrentElement { get => currentElement; set => currentElement = value; }
        public int NumberOfElements { get => numberOfElements; set => numberOfElements = value; }

        /// <summary> Returns the current element to be set. </summary>
        public JumperDisplayElement GetCurrentElement()
        {
            if (currentElement < numberOfElements)
                return elements[currentElement];
            else
                return elements[numberOfElements - 1];

        }

        /// <summary> Return a field at specified index. Returns null if index doesnt exist. </summary>
        public JumperDisplayElement GetFieldAtIndex(int n)
        {
            if (n < numberOfElements)
                return elements[n];

            return null;
        }

        /// <summary> Shows all the elements in row. </summary>
        public override void Show()
        {
            foreach (JumperDisplayElement element in elements)
                element.Show();
        }

        /// <summary> Updates the Image of the current element. </summary>
        public override void Update<T>(T message)
        {
            if (message is Image || message == null)
                elements[currentElement].Update(message);
            else
                throw new Exception("This function only accepts Images");
        }
    }

    /// <summary>
    /// GUIElement used for representing a field of a cheker used for displaying the validity of a element of the answer in the Jumper Game.
    /// </summary>
    public class JumperCheckerElement : GUIElement
    {
        Control field;
        Color color;

        public JumperCheckerElement(Control field)
        {
            this.field = field;

            color = Color.Gray;
        }

        /// <summary> Shows the element. </summary>
        public override void Show()
        {
            field.BackColor = color;

            field.Refresh();
        }

        /// <summary> Updates the color of the element. </summary>
        public override void Update<T>(T message)
        {
            if (message is Color)
                color = (Color)(message as object);
            else
                throw new Exception("This function only accepts Colors");
        }
    }

    /// <summary>
    /// GUIElement used for representing the validity of the whole answer in the Jumper Game.
    /// </summary>
    public class JumperCheckerComponent : GUIElement
    {
        JumperCheckerElement[] elements;

        int numberOfElements;

        public JumperCheckerComponent(JumperCheckerElement[] elements, int numberOfElements = 4)
        {
            this.elements = elements;

            this.numberOfElements = numberOfElements;
        }

        /// <summary> Shows the whole componenet. </summary>
        public override void Show()
        {
            foreach (JumperCheckerElement element in elements)
                element.Show();
        }

        /// <summary> Set the colors of every element in the comonenet. </summary>
        public override void Update<T>(T message)
        {
            if (message is Color[])
            {
                Color[] colorArray = message as Color[];
                for (int i = 0; i < numberOfElements; i++)
                    elements[i].Update(colorArray[i]);
            }
            else
                throw new Exception("This function only accepts Color arrays");
        }
    }

    /// <summary>
    /// GUIElement used for representing the whole Jumper Game display.
    /// </summary>
    public class JumperDisplay : GUIElement
    {
        JumperDisplayComponent[] displays;
        JumperCheckerComponent[] checkers;

        int numberOfRows;
        int currentRow;

        public JumperDisplay(JumperDisplayComponent[] displays, JumperCheckerComponent[] checkers, int numberOfRows = 6)
        {
            this.displays = displays;
            this.checkers = checkers;

            this.numberOfRows = numberOfRows;
        }

        public int CurrentRow { get => currentRow; set => currentRow = value; }

        /// <summary> Manualy show the checker. (TODO: remove the need for this function) </summary>
        public void ManualChekerShow()
        {
            checkers[currentRow].Show();
        }

        /// <summary> Returns the current element to be set. </summary>
        public JumperDisplayElement GetActiveElement()
        {
            return displays[currentRow].GetCurrentElement();
        }

        /// <summary> Returns the current row. </summary>
        public JumperDisplayComponent GetCurrentRow()
        {
            return displays[currentRow];
        }

        /// <summary> Shows the displays and checkers. </summary>
        public override void Show()
        {
            if (currentRow < numberOfRows)
            {
                displays[currentRow].Show();
                checkers[currentRow].Show();
            }
        }

        /// <summary> Sets the displays or checker depending on the type of the message. </summary>
        public override void Update<T>(T message)
        {
            if (message is Image || message == null)
                displays[currentRow].Update(message);
            else if (message is Color[])
                checkers[currentRow].Update(message);
            else
                throw new Exception("This function only accepts Images of Color arrays");
        }
    }

}
