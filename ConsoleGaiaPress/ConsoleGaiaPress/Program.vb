Imports System
Imports System.IO
Imports System.Diagnostics
Imports Microsoft.Web.WebView2.Core

Module Program
    Sub Main()
        ' ✅ Vérifier si WebView2 Runtime est installé
        Dim browserVersion As String = CoreWebView2Environment.GetAvailableBrowserVersionString()
        If String.IsNullOrEmpty(browserVersion) Then
            Console.WriteLine("WebView2 Runtime n'est pas installé. Veuillez l'installer depuis https://go.microsoft.com/fwlink/?linkid=2148294")
            Console.WriteLine("Appuyez sur Entrée pour quitter...")
            Console.ReadLine()
            Return
        End If

        ' ✅ Chemin vers le répertoire wwwroot
        Dim projectRoot As String = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\..\.."))
        Dim wwwrootPath As String = Path.Combine(projectRoot, "wwwroot\html")

        ' ✅ Copier Menu_Principal.html avec un nom unique
        Dim menuSourcePath As String = Path.Combine(wwwrootPath, "Menu_Principal.html")
        Dim menuUniqueFileName As String = $"Menu_Principal_{DateTime.Now:yyyyMMddHHmmss}.html"
        Dim menuDestPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, menuUniqueFileName)

        ' ✅ Copier Nouveau_Forage.html avec un nom unique (si nécessaire)
        Dim forageSourcePath As String = Path.Combine(wwwrootPath, "Nouveau_Forage.html")
        'Dim forageUniqueFileName As String = $"Nouveau_Forage_{DateTime.Now:yyyyMMddHHmmss}.html"
        Dim forageUniqueFileName As String = $"Nouveau_Forage.html"
        Dim forageDestPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, forageUniqueFileName)

        ' ✅ Vérifier et copier Menu_Principal.html
        If File.Exists(menuSourcePath) Then
            If File.Exists(menuDestPath) Then
                File.Delete(menuDestPath)
            End If
            File.Copy(menuSourcePath, menuDestPath, True)
            Console.WriteLine($"Menu_Principal.html copié vers : {menuDestPath}")
        Else
            Console.WriteLine($"Erreur : Le fichier {menuSourcePath} est introuvable.")
            Console.WriteLine("Appuyez sur Entrée pour quitter...")
            Console.ReadLine()
            Return
        End If

        ' ✅ Vérifier et copier Nouveau_Forage.html (si nécessaire)
        If File.Exists(forageSourcePath) Then
            If File.Exists(forageDestPath) Then
                File.Delete(forageDestPath)
            End If
            File.Copy(forageSourcePath, forageDestPath, True)
            Console.WriteLine($"Nouveau_Forage.html copié vers : {forageDestPath}")
        End If

        ' ✅ Ouvrir Menu_Principal.html dans le navigateur
        Process.Start(New ProcessStartInfo With {
            .FileName = menuDestPath,
            .UseShellExecute = True
        })
    End Sub
End Module