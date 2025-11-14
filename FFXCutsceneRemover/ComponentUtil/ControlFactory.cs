using System;
using System.Drawing;
using System.Windows.Forms;

namespace FFXCutsceneRemover.ComponentUtil;

/// <summary>
/// Factory methods for creating common WinForms controls.
/// Reduces boilerplate in UI initialization.
/// </summary>
public static class ControlFactory
{
    /// <summary>
    /// Creates a label with the specified properties.
    /// </summary>
    /// <param name="text">Label text</param>
    /// <param name="location">Label position</param>
    /// <param name="size">Label size (null for auto-size)</param>
    /// <returns>Configured Label control</returns>
    public static Label CreateLabel(string text, Point location, Size? size = null)
    {
        return new Label
        {
            Text = text,
            Location = location,
            Size = size ?? new Size(100, 20),
            AutoSize = size == null
        };
    }

    /// <summary>
    /// Creates a button with the specified properties and click handler.
    /// </summary>
    /// <param name="text">Button text</param>
    /// <param name="location">Button position</param>
    /// <param name="size">Button size</param>
    /// <param name="clickHandler">Click event handler (optional)</param>
    /// <returns>Configured Button control</returns>
    public static Button CreateButton(string text, Point location, Size size, EventHandler clickHandler = null)
    {
        var button = new Button
        {
            Text = text,
            Location = location,
            Size = size
        };

        if (clickHandler != null)
        {
            button.Click += clickHandler;
        }

        return button;
    }

    /// <summary>
    /// Creates a checkbox with the specified properties.
    /// </summary>
    /// <param name="text">Checkbox text</param>
    /// <param name="location">Checkbox position</param>
    /// <param name="isChecked">Initial checked state</param>
    /// <param name="changedHandler">CheckedChanged event handler (optional)</param>
    /// <returns>Configured CheckBox control</returns>
    public static CheckBox CreateCheckBox(string text, Point location, bool isChecked = false, EventHandler changedHandler = null)
    {
        var checkBox = new CheckBox
        {
            Text = text,
            Location = location,
            AutoSize = true,
            Checked = isChecked
        };

        if (changedHandler != null)
        {
            checkBox.CheckedChanged += changedHandler;
        }

        return checkBox;
    }

    /// <summary>
    /// Creates a numeric up/down control with the specified properties.
    /// </summary>
    /// <param name="location">Control position</param>
    /// <param name="size">Control size</param>
    /// <param name="minimum">Minimum value</param>
    /// <param name="maximum">Maximum value</param>
    /// <param name="value">Initial value</param>
    /// <returns>Configured NumericUpDown control</returns>
    public static NumericUpDown CreateNumericUpDown(Point location, Size size, decimal minimum, decimal maximum, decimal value)
    {
        return new NumericUpDown
        {
            Location = location,
            Size = size,
            Minimum = minimum,
            Maximum = maximum,
            Value = value
        };
    }

    /// <summary>
    /// Creates a combo box with the specified properties.
    /// </summary>
    /// <param name="location">Control position</param>
    /// <param name="size">Control size</param>
    /// <param name="items">Items to populate the combo box</param>
    /// <param name="selectedIndex">Initial selected index</param>
    /// <returns>Configured ComboBox control</returns>
    public static ComboBox CreateComboBox(Point location, Size size, object[] items, int selectedIndex = 0)
    {
        var comboBox = new ComboBox
        {
            Location = location,
            Size = size,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        comboBox.Items.AddRange(items);

        if (selectedIndex >= 0 && selectedIndex < items.Length)
        {
            comboBox.SelectedIndex = selectedIndex;
        }

        return comboBox;
    }

    /// <summary>
    /// Creates a text box with the specified properties.
    /// </summary>
    /// <param name="location">Control position</param>
    /// <param name="size">Control size</param>
    /// <param name="text">Initial text</param>
    /// <param name="readOnly">Whether the text box is read-only</param>
    /// <returns>Configured TextBox control</returns>
    public static TextBox CreateTextBox(Point location, Size size, string text = "", bool readOnly = false)
    {
        return new TextBox
        {
            Location = location,
            Size = size,
            Text = text,
            ReadOnly = readOnly
        };
    }

    /// <summary>
    /// Creates a group box with the specified properties.
    /// </summary>
    /// <param name="text">Group box title</param>
    /// <param name="location">Control position</param>
    /// <param name="size">Control size</param>
    /// <returns>Configured GroupBox control</returns>
    public static GroupBox CreateGroupBox(string text, Point location, Size size)
    {
        return new GroupBox
        {
            Text = text,
            Location = location,
            Size = size
        };
    }

    /// <summary>
    /// Creates a radio button with the specified properties.
    /// </summary>
    /// <param name="text">Radio button text</param>
    /// <param name="location">Radio button position</param>
    /// <param name="size">Radio button size</param>
    /// <param name="isChecked">Initial checked state</param>
    /// <param name="changedHandler">CheckedChanged event handler (optional)</param>
    /// <returns>Configured RadioButton control</returns>
    public static RadioButton CreateRadioButton(string text, Point location, Size size, bool isChecked = false, EventHandler changedHandler = null)
    {
        var radioButton = new RadioButton
        {
            Text = text,
            Location = location,
            Size = size,
            Checked = isChecked
        };

        if (changedHandler != null)
        {
            radioButton.CheckedChanged += changedHandler;
        }

        return radioButton;
    }

    /// <summary>
    /// Creates a link label with the specified properties.
    /// </summary>
    /// <param name="text">Link label text</param>
    /// <param name="location">Link label position</param>
    /// <param name="size">Link label size</param>
    /// <param name="clickHandler">LinkClicked event handler (optional)</param>
    /// <returns>Configured LinkLabel control</returns>
    public static LinkLabel CreateLinkLabel(string text, Point location, Size size, LinkLabelLinkClickedEventHandler clickHandler = null)
    {
        var linkLabel = new LinkLabel
        {
            Text = text,
            Location = location,
            Size = size
        };

        if (clickHandler != null)
        {
            linkLabel.LinkClicked += clickHandler;
        }

        return linkLabel;
    }

    /// <summary>
    /// Creates a rich text box with the specified properties.
    /// </summary>
    /// <param name="location">Control position</param>
    /// <param name="size">Control size</param>
    /// <param name="readOnly">Whether the text box is read-only</param>
    /// <returns>Configured RichTextBox control</returns>
    public static RichTextBox CreateRichTextBox(Point location, Size size, bool readOnly = false)
    {
        return new RichTextBox
        {
            Location = location,
            Size = size,
            ReadOnly = readOnly
        };
    }
}

/// <summary>
/// Extension methods for fluent control configuration.
/// Enables chainable configuration of WinForms controls.
/// </summary>
public static class ControlFactoryExtensions
{
    /// <summary>
    /// Adds a tooltip to a control.
    /// </summary>
    /// <typeparam name="T">Control type</typeparam>
    /// <param name="control">The control to configure</param>
    /// <param name="tooltip">The ToolTip component</param>
    /// <param name="message">The tooltip message</param>
    /// <returns>The control for method chaining</returns>
    public static T WithTooltip<T>(this T control, ToolTip tooltip, string message) where T : Control
    {
        tooltip.SetToolTip(control, message);
        return control;
    }

    /// <summary>
    /// Sets the size of a control.
    /// </summary>
    /// <typeparam name="T">Control type</typeparam>
    /// <param name="control">The control to configure</param>
    /// <param name="width">Width in pixels</param>
    /// <param name="height">Height in pixels</param>
    /// <returns>The control for method chaining</returns>
    public static T WithSize<T>(this T control, int width, int height) where T : Control
    {
        control.Size = new Size(width, height);
        return control;
    }

    /// <summary>
    /// Sets the size of a control.
    /// </summary>
    /// <typeparam name="T">Control type</typeparam>
    /// <param name="control">The control to configure</param>
    /// <param name="size">The size</param>
    /// <returns>The control for method chaining</returns>
    public static T WithSize<T>(this T control, Size size) where T : Control
    {
        control.Size = size;
        return control;
    }

    /// <summary>
    /// Sets the anchor style of a control.
    /// </summary>
    /// <typeparam name="T">Control type</typeparam>
    /// <param name="control">The control to configure</param>
    /// <param name="anchor">The anchor style</param>
    /// <returns>The control for method chaining</returns>
    public static T WithAnchor<T>(this T control, AnchorStyles anchor) where T : Control
    {
        control.Anchor = anchor;
        return control;
    }

    /// <summary>
    /// Sets the enabled state of a control.
    /// </summary>
    /// <typeparam name="T">Control type</typeparam>
    /// <param name="control">The control to configure</param>
    /// <param name="enabled">Whether the control is enabled</param>
    /// <returns>The control for method chaining</returns>
    public static T WithEnabled<T>(this T control, bool enabled) where T : Control
    {
        control.Enabled = enabled;
        return control;
    }

    /// <summary>
    /// Sets the font of a control.
    /// </summary>
    /// <typeparam name="T">Control type</typeparam>
    /// <param name="control">The control to configure</param>
    /// <param name="font">The font</param>
    /// <returns>The control for method chaining</returns>
    public static T WithFont<T>(this T control, Font font) where T : Control
    {
        control.Font = font;
        return control;
    }

    /// <summary>
    /// Sets the foreground color of a control.
    /// </summary>
    /// <typeparam name="T">Control type</typeparam>
    /// <param name="control">The control to configure</param>
    /// <param name="color">The foreground color</param>
    /// <returns>The control for method chaining</returns>
    public static T WithForeColor<T>(this T control, Color color) where T : Control
    {
        control.ForeColor = color;
        return control;
    }

    /// <summary>
    /// Sets the background color of a control.
    /// </summary>
    /// <typeparam name="T">Control type</typeparam>
    /// <param name="control">The control to configure</param>
    /// <param name="color">The background color</param>
    /// <returns>The control for method chaining</returns>
    public static T WithBackColor<T>(this T control, Color color) where T : Control
    {
        control.BackColor = color;
        return control;
    }

    /// <summary>
    /// Sets the text alignment of a label.
    /// </summary>
    /// <param name="label">The label to configure</param>
    /// <param name="alignment">The content alignment</param>
    /// <returns>The label for method chaining</returns>
    public static Label WithTextAlign(this Label label, ContentAlignment alignment)
    {
        label.TextAlign = alignment;
        return label;
    }

    /// <summary>
    /// Sets the text alignment of a link label.
    /// </summary>
    /// <param name="linkLabel">The link label to configure</param>
    /// <param name="alignment">The content alignment</param>
    /// <returns>The link label for method chaining</returns>
    public static LinkLabel WithTextAlign(this LinkLabel linkLabel, ContentAlignment alignment)
    {
        linkLabel.TextAlign = alignment;
        return linkLabel;
    }

    /// <summary>
    /// Sets the link colors of a link label.
    /// </summary>
    /// <param name="linkLabel">The link label to configure</param>
    /// <param name="linkColor">Normal link color</param>
    /// <param name="activeLinkColor">Active link color</param>
    /// <param name="visitedLinkColor">Visited link color</param>
    /// <returns>The link label for method chaining</returns>
    public static LinkLabel WithLinkColors(this LinkLabel linkLabel, Color linkColor, Color activeLinkColor, Color visitedLinkColor)
    {
        linkLabel.LinkColor = linkColor;
        linkLabel.ActiveLinkColor = activeLinkColor;
        linkLabel.VisitedLinkColor = visitedLinkColor;
        return linkLabel;
    }
}
