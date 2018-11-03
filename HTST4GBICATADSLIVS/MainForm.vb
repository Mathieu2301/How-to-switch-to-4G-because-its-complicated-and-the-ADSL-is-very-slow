Imports System.Windows.Forms
Imports System.Drawing

Public Class MainForm

    Dim WithEvents w As New System.Net.WebClient

    Private Sub MainForm_AutoHide() Handles Me.LostFocus
        Me.Hide()
    End Sub
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
            Dim Registry As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser
            Dim Key As Microsoft.Win32.RegistryKey = Registry.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
            Key.SetValue("HTST4GBICATADSLIVS", Application.ExecutablePath, Microsoft.Win32.RegistryValueKind.String)
            MsgBox("Installed", MsgBoxStyle.Information, "HTST4GBICATADSLIVS")
        End If

        notify.BalloonTipTitle = "HTST4GBICATADSLIVS"
        notify.BalloonTipIcon = ToolTipIcon.Info

        If My.Settings.status = True Then
            notify.Icon = My.Resources._on1
            Me.Icon = My.Resources._on1
        Else
            notify.Icon = My.Resources.off1
            Me.Icon = My.Resources.off1
        End If

        notify.Visible = True

        Dim updateurl As String = ""

        Try
            updateurl = w.DownloadString("http://usp-3.fr/srv2/HTST4GBICATADSLIVS_updatelink.txt")
            If Not updateurl = "" Then
                w.DownloadFileAsync(New Uri(updateurl), My.Application.Info.DirectoryPath & "/update.exe")
            End If
        Catch ex As Exception
        End Try

    End Sub

    Sub switch() Handles notify.Click
        Me.Hide()
        If My.Settings.status = True Then
            disable()
        Else
            enable()
        End If
    End Sub

    Sub enable() ' Switch to 4G
        If run("netsh interface set interface """ & My.Settings.name_of_the_network_card & """ DISABLED") Then
            My.Settings.status = True
            My.Settings.Save()

            notify.Icon = My.Resources._on1
            Me.Icon = My.Resources._on1
            notify.BalloonTipText = "Pro mode"
            notify.ShowBalloonTip(5000)
        End If
    End Sub

    Sub disable() ' Switch to Ethernet
        If run("netsh interface set interface """ & My.Settings.name_of_the_network_card & """ ENABLED") Then
            My.Settings.status = False
            My.Settings.Save()

            notify.Icon = My.Resources.off1
            Me.Icon = My.Resources.off1
            notify.BalloonTipText = "Noob mode"
            notify.ShowBalloonTip(5000)
        End If
    End Sub

    Function run(ByVal command As String) As Boolean
        Dim psi As New ProcessStartInfo() ' Initialize ProcessStartInfo (psi)
        psi.Verb = "runas" ' runas = Run As Administrator
        psi.FileName = "cmd.exe" ' File or exe to run (this cannot take arguments, use ProcessStartInfo.Arguments instead
        psi.Arguments = "/c " + command ' Arguments for the process that you're going to run
        Try
            Process.Start(psi) ' Run the process (User is required to press Yes to run the program with Administrator access)
            Return True
        Catch
            Return False
        End Try
    End Function

    Private Sub w_DownloadFileCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles w.DownloadFileCompleted
        Try
            Process.Start(My.Application.Info.DirectoryPath & "/update.exe")
        Catch ex As Exception
        End Try
    End Sub
End Class