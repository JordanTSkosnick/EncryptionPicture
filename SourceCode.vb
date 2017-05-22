Public Class Form1
    'once this button is clicked, it will encode the message'
    Private Sub btnEncode_Click(sender As Object, e As EventArgs) Handles btnEncode.Click
       Dim mesgBits As String
       Dim inImageFile As String = txtCleanimage.Text
       Dim outImageFile As String = "encStegImage"
       Dim plaintext As String = txtPlaintext.Text
       Dim interval As Integer = txtKey.Text
       plaintext = My.Computer.FileSystem.ReadAllText("U:/ProjectOne/" + plaintext + ".txt")
       mesgBits = bitify(plaintext)
       hideIt(interval, mesgBits, inImageFile, outImageFile)
       display.Text = "Encoding done."
    End Sub

    'turns the message into bits to be stored into the pixels'
    Public Function bitify(ByVal message As String) As String
       Dim i As Integer
       Dim bitString As String
       txtMesgLen.Text = Len(message)
       bitString = ""
 
       For i = 0 To Len(message) - 1
           bitString += intToBits(Asc(message(i)), 8)
       Next
       Return bitString
 
    End Function

    Public Function intToBits(ByVal oneInt As Integer, ByVal sLen As Integer) As String
       Dim bitString As String
       Dim bit As String
       bitString = ""
 
       For i = 0 To sLen - 1
           bit = Trim(Str(oneInt Mod 2))
           bitString += bit
           oneInt = oneInt \ 2
       Next
 
       Return StrReverse(bitString)
 
    End Function

    'stores a message bit into a pixel'
    Public Sub hideIt(ByVal interval As Integer, ByVal mesgBits As String, ByVal inImageFile As String, ByVal outImageFile As String)
       Dim picLoc As String = "U:/ProjectOne/"
       Dim imgWidth, imgHeight, x, y As Integer
       Dim bitChar As String
       Dim pixelColor, newPixelColor As Color
       Dim bitMapImg As New Bitmap(picLoc + inImageFile + ".jpg")
       imgWidth = bitMapImg.Width
       imgHeight = bitMapImg.Height
 
       For i = 0 To Len(mesgBits) - 1
         
           pixelColor = bitMapImg.GetPixel(x, y)
           bitChar = mesgBits(i)
           newPixelColor = setCiphPixel(pixelColor, bitChar)
           bitMapImg.SetPixel(x, y, newPixelColor)
           x += interval
           If x >= imgWidth Then
               y += 1
               x = x - imgWidth
           End If
        
       Next
       bitMapImg.Save(picLoc + outImageFile + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp)
    End Sub

    'sets the pixel that will store a bit from the message'
    Public Function setCiphPixel(ByVal pixie As Color, ByVal bitVal As Integer) As Color
       Dim newColor As Color
       Dim pixR As Integer = pixie.R
       If bitVal = 0 Then
           If pixR Mod 2 = 1 Then
               pixR -= 1
           End If
       Else
           If pixR Mod 2 = 0 Then
               pixR += 1
           End If
       End If
       newColor = Color.FromArgb(pixie.A, pixR, pixie.G, pixie.B)
       Return newColor
    End Function
    
    'Once this button is clicked, it will extract/decode the message'
    Private Sub btnDecode_Click(sender As System.Object, e As System.EventArgs) Handles btnDecode.Click
       Dim rcvdFile = txtRecoveredplaintext.Text
       Dim interval As Integer = txtKey.Text
       Dim mesgLen As Integer = txtMesgLen.Text
       Dim result As String = extractMsg(txtCarrierimagefile.Text, mesgLen, interval)
       writeIt(result, rcvdFile)
    End Sub
   
    'Goes through the image and extracts the message
    Public Function extractMsg(ByVal imageFile As String, ByVal mesgLen As Integer, ByVal interval As Integer) As String
       Dim picLoc As String = "U:/ProjectOne/"
       Dim imgWidth, imgHeight, x, y, i, j, nxBit, ciphInt As Integer
       Dim rcvdMsg As String = " "
       Dim pixelColor As Color
       Dim bitMapImg As New Bitmap(picLoc + imageFile + ".bmp")
       imgWidth = bitMapImg.Width
       imgHeight = bitMapImg.Height
       x = 0
       y = 0
       For i = 0 To mesgLen - 1
           ciphInt = 0

           For j = 0 To 7
               pixelColor = bitMapImg.GetPixel(x, y)
               ciphInt = ciphInt * 2
               nxBit = extractBit(pixelColor)
               ciphInt = ciphInt + nxBit
               x += interval
               If x >= imgWidth Then
                   y += 1
                   x = x - imgWidth
               End If
 
           Next
           rcvdMsg += Chr(ciphInt)
       Next
       Return rcvdMsg
    End Function

    'Writes the message into a new file.
    Public Sub writeIt(ByVal message As String, ByVal rcvdFile As String)
       Dim folder = "U:/ProjectOne/"
       Dim writeFile = folder + rcvdFile + ".txt"
       Dim fw As System.IO.StreamWriter
       fw = My.Computer.FileSystem.OpenTextFileWriter(writeFile, False)
       fw.Write(message)
       fw.Close()
       display.Text = "Writing done"
    End Sub

    'Extracts the bit within the pixel'
    Public Function extractBit(ByVal pixie As Color) As Integer
       Dim pixR As Integer
       pixR = pixie.R
       Return pixR Mod 2
    End Function
 
   
End Class
 

