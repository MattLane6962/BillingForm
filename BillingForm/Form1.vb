' Program Name:     Ch05HandsOn
' Programmer:       Matthew Lane
' Date:             3/11/2016
' Decription:       This Project Calculates the amount due
'                   based on the customer selection
'                   and accumulates summary data for the day.
'                   Incorporates menus and common dialog boxes,
'                   which allow the user to change the font and
'                   color of the text boxes.


Public Class BillingForm

    ' Declare module-level constants.
    Const TAX_RATE_Decimal As Decimal = 0.08D
    Const CAPPUCCINO_PRICE_Decimal As Decimal = 2D
    Const ESPRESSO_PRICE_Decimal As Decimal = 2.25D
    Const LATTE_PRICE_Decimal As Decimal = 1.75D
    Const ICED_PRICE_Decimal As Decimal = 2.5D

    ' Declare module-level variables for summary information.
    Private SubtotalDecimal, TotalDecimal, GrandTotalDecimal As Decimal
    Private CustomerCountInteger As Integer




    Private Sub CalculateButton_Click(sender As Object, e As EventArgs) Handles CalculateButton.Click,
        CalculateSelectionToolStripMenuItem.Click
        ' Calculate and disply the current amounts and add to totals.

        Dim PriceDecimal, TaxDecimal, ItemAmountDecimal As Decimal
        Dim QuantityInteger As Integer

        ' Find the price.
        If CappuccinoRadioButton.Checked Then
            PriceDecimal = CAPPUCCINO_PRICE_Decimal
        ElseIf EspressoRadioButton.Checked Then
            PriceDecimal = ESPRESSO_PRICE_Decimal
        ElseIf LatteRadioButton.Checked Then
            PriceDecimal = LATTE_PRICE_Decimal
        ElseIf IcedCappuccinoRadioButton.Checked Or IcedLatteRadioButton.Checked Then
            PriceDecimal = ICED_PRICE_Decimal
        End If

        ' Calculate extended price and add to order total.
        Try
            QuantityInteger = Integer.Parse(QuantityTextBox.Text)
            ItemAmountDecimal = PriceDecimal * QuantityInteger
            SubtotalDecimal += ItemAmountDecimal
            If TaxCheckBox.Checked Then
                TaxDecimal = FindTax(SubtotalDecimal)
            Else
                TaxDecimal = 0
            End If
            TotalDecimal = SubtotalDecimal + TaxDecimal
            ItemAmountTextBox.Text = ItemAmountDecimal.ToString("C")
            SubTotalTextBox.Text = SubtotalDecimal.ToString("C")
            TaxTextBox.Text = TaxDecimal.ToString("C")
            TotalTextBox.Text = TotalDecimal.ToString("C")
            ' Allow a change only for a new order.
            TaxCheckBox.Enabled = False
            ' Allow clear after an order has begun.
            ClearButton.Enabled = True
            NewOrderToolStripMenuItem.Enabled = True
            NewOrderToolStripMenuItem.Enabled = True
        Catch QuantityException As FormatException
            MessageBox.Show("Quantity must be numeric.", "Data Entry Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            With QuantityTextBox
                .Focus()
                .SelectAll()
            End With
        End Try
    End Sub

    Private Function FindTax(ByVal AmountDecimal As Decimal) As Decimal
        ' Calculate the sales tax.

        Return AmountDecimal * TAX_RATE_Decimal
    End Function

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click,
        ClearItemToolStripMenuItem.Click
        ' Clear the appropriate controls.
        CappuccinoRadioButton.Checked = True ' All others are false.
        ItemAmountTextBox.Clear()
        With QuantityTextBox
            .Clear()
            .Focus()
        End With
    End Sub

    Private Sub NewOrderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewOrderToolStripMenuItem.Click
        ' Clear the current order and add to the totals.

        Dim ReturnDialogResult As DialogResult
        Dim MessageString As String

        ' Confirm clear of the current order. 
        MessageString = "Clear the current order figures?"
        ReturnDialogResult = MessageBox.Show(MessageString, "Clear order",
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                             MessageBoxDefaultButton.Button2)

        If ReturnDialogResult = Windows.Forms.DialogResult.Yes Then
            ' User said Yes; Clear the screen fields.
            ClearButton_Click(sender, e)
            SubTotalTextBox.Clear()
            TaxTextBox.Clear()
            TotalTextBox.Clear()

            ' Add to totals.
            ' Add only if not a new order/customer.
            If SubtotalDecimal <> 0 Then
                GrandTotalDecimal += TotalDecimal
                CustomerCountInteger += 1
                ' Reset totals for the next customer. 
                SubtotalDecimal = 0
                TotalDecimal = 0
            End If

            ' Clear appropriate display items and enable the check box.
            With TaxCheckBox
                .Enabled = True
                .Checked = False
            End With
            ClearButton.Enabled = False
            NewOrderToolStripMenuItem.Enabled = False
        End If
    End Sub

    Private Sub SummaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SummaryToolStripMenuItem.Click
        ' Calculate the average and display the totals.

        Dim AverageDecimal As Decimal
        Dim MessageString As String

        If TotalDecimal <> 0 Then
            ' Make sure the last order is counted.
            NewOrderToolStripMenuItem_Click(sender, e)
            ' Pass incoming arguments to the called procedure.
        End If

        If CustomerCountInteger > 0 Then
            'Calculate the average
            AverageDecimal = GrandTotalDecimal / CustomerCountInteger

            'Concatenate the message string.
            MessageString = "Number of orders: " &
                CustomerCountInteger.ToString() &
                Environment.NewLine & Environment.NewLine &
                "Total Sales: " & GrandTotalDecimal.ToString("C") &
                Environment.NewLine & Environment.NewLine &
                "Average Sale: " & AverageDecimal.ToString("C")
            MessageBox.Show(MessageString, "Coffee Sales Summary",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageString = "No sales data to summarize."
            MessageBox.Show(MessageString, "Coffee Sales Summary", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        ' Display the About message box.
        Dim MessageString As String

        MessageString = "R 'n R Billing" & Environment.NewLine & Environment.NewLine &
            "Programmed by Matthew Lane."
        MessageBox.Show(MessageString, "About R 'n R Billing", MessageBoxButtons.OK,
                        MessageBoxIcon.Information)
    End Sub

    Private Sub FontToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles FontToolStripMenuItem1.Click
        ' Allows the user to select a new font for the summary totals.

        With FontDialog1
            .Font = SubTotalTextBox.Font
            .ShowDialog()
            SubTotalTextBox.Font = .Font
            TaxTextBox.Font = .Font
            TotalTextBox.Font = .Font
        End With
    End Sub

    Private Sub ColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ColorToolStripMenuItem.Click
        ' Allow the user to select a new color for the summary totals.

        With ColorDialog1
            .Color = SubTotalTextBox.ForeColor
            .ShowDialog()
            SubTotalTextBox.ForeColor = .Color
            TaxTextBox.ForeColor = .Color
            TotalTextBox.ForeColor = .Color
        End With
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem1.Click
        ' Terminate the project.

        Me.Close()
    End Sub
End Class
