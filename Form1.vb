Imports System.IO
Imports System.Net
Public Class Form1
    Dim path As String = Application.StartupPath() + "\DownloadedApps\"
    Dim confirmInstall As Boolean = True
    Sub DownloadIt(ByRef btn)
        Dim _button As Button = btn
        Dim url = btn.Tag
        Dim programName = btn.Text
        Dim nome = My.Computer.FileSystem.GetName(url)
        Dim dlPath As String = path + nome
        _button.Enabled = False
        _button.BackColor = Color.FromArgb(255, 128, 128)
        If Not My.Computer.FileSystem.FileExists(dlPath) Then
            Try
                Dim wc As New WebClient()

                AddHandler wc.DownloadFileCompleted,
                    Sub()
                        _button.Enabled = True
                        _button.BackColor = Color.FromArgb(128, 255, 128)
                        If confirmInstall Then ShowDialog(programName, dlPath, _button)

                    End Sub

                wc.UseDefaultCredentials = True
                ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)

                wc.DownloadFileAsync(New Uri(url), dlPath)

            Catch ex As Exception
                MsgBox(ex.Message)

            End Try

        Else
            Shell("explorer /select, " + dlPath, AppWinStyle.NormalFocus)
            _button.Enabled = True
            _button.BackColor = Color.FromArgb(128, 255, 128)
        End If


    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Directory.Exists(path) Then
            Directory.CreateDirectory(path)
        End If
        For Each gp As Control In Me.Controls
            For Each con As Control In gp.Controls
                If con.GetType Is GetType(Button) Then
                    If File.Exists(path + My.Computer.FileSystem.GetName(con.Tag)) Then
                        con.BackColor = Color.FromArgb(128, 255, 128)
                    End If
                    AddHandler con.Click, Sub() DownloadIt(con)
                End If
            Next
        Next
    End Sub

    Private Sub DownloadFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DownloadFolderToolStripMenuItem.Click
        Shell("explorer " + path, AppWinStyle.NormalFocus)
    End Sub
    Sub ShowDialog(ByRef programName As String, ByRef dlPath As String, ByVal btn As Button)
        Select Case MsgBox("Deseja instalar agora?", MsgBoxStyle.YesNo, programName + " - Download concluído")
            Case MsgBoxResult.Yes
                Process.Start(dlPath, AppWinStyle.NormalFocus)
            Case MsgBoxResult.No
        End Select
    End Sub

    Private Sub GitHubToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GitHubToolStripMenuItem.Click
        Process.Start("https://github.com/mococa/downloadmanager")
    End Sub

    Private Sub AskForInstallationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AskForInstallationToolStripMenuItem.Click
        Dim result As DialogResult = MessageBox.Show("Do you want to receive an installation
                                                     confirmation every time you download something?",
                                                     "Installation Confirmation",
                                                     MessageBoxButtons.YesNoCancel)
        If result = DialogResult.Cancel Then
            Return
        ElseIf result = DialogResult.No Then
            confirmInstall = False
        ElseIf result = DialogResult.Yes Then
            confirmInstall = True
        End If

    End Sub
End Class