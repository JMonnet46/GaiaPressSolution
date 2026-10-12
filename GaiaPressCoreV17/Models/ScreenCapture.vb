'Imports System.Drawing
Imports System.Runtime.InteropServices

Public Class ScreenCapture
    <DllImport("user32.dll")>
    Private Shared Function GetDC(hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function ReleaseDC(hWnd As IntPtr, hDC As IntPtr) As Integer
    End Function

    '<DllImport("gdi32.dll")>
    'Private Shared Function BitBlt(hdcDest As IntPtr, xDest As Integer, yDest As Integer,
    '                              width As Integer, height As Integer,
    '                              hdcSrc As IntPtr, xSrc As Integer, ySrc As Integer,
    '                              rop As Integer) As Boolean
    'End Function

    '<DllImport("gdi32.dll")>
    'Private Shared Function CreateCompatibleDC(hdc As IntPtr) As IntPtr
    'End Function

    '<DllImport("gdi32.dll")>
    'Private Shared Function CreateCompatibleBitmap(hdc As IntPtr, width As Integer, height As Integer) As IntPtr
    'End Function

    '<DllImport("gdi32.dll")>
    'Private Shared Function SelectObject(hdc As IntPtr, obj As IntPtr) As IntPtr
    'End Function

    '<DllImport("gdi32.dll")>
    'Private Shared Function DeleteObject(obj As IntPtr) As Boolean
    'End Function

    ''Public Shared Function CaptureScreen() As Bitmap
    ''Dim screenWidth As Integer = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width
    ''Dim screenHeight As Integer = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height

    'Dim hScreenDC As IntPtr = GetDC(IntPtr.Zero)
    '    Dim hMemoryDC As IntPtr = CreateCompatibleDC(hScreenDC)

    '    'Dim hBitmap As IntPtr = CreateCompatibleBitmap(hScreenDC, screenWidth, screenHeight)
    '    Dim hOldBitmap As IntPtr = SelectObject(hMemoryDC, hBitmap)

    '    BitBlt(hMemoryDC, 0, 0, screenWidth, screenHeight, hScreenDC, 0, 0, &HCC0020) ' SRCCOPY

    '    ' Créer un Bitmap .NET à partir du handle
    '    ''Dim bmp As New Bitmap(screenWidth, screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
    '    'Using g As Graphics = Graphics.FromImage(bmp)
    '    '    Dim hOldBmp As IntPtr = SelectObject(hMemoryDC, hOldBitmap)
    '    '    g.CopyFromScreen(New Point(0, 0), New Point(0, 0), New Size(screenWidth, screenHeight))
    '    '    SelectObject(hMemoryDC, hOldBmp)
    '    'End Using

    '    ' Nettoyer
    '    ReleaseDC(IntPtr.Zero, hScreenDC)
    '    DeleteObject(hBitmap)
    '    DeleteObject(hOldBitmap)

    '    Return bmp
    'End Function
End Class