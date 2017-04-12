Imports System
Imports RegawMOD.Android
Imports System.IO
Public Class Form1
    Dim android As AndroidController
    Dim device As Device
    Public Sub Delay(ByVal DelayInSeconds As Integer)
        Dim ts As TimeSpan
        Dim targetTime As DateTime = DateTime.Now.AddSeconds(DelayInSeconds)
        Do
            ts = targetTime.Subtract(DateTime.Now)
            Application.DoEvents() ' keep app responsive
            System.Threading.Thread.Sleep(50) ' reduce CPU usage
        Loop While ts.TotalSeconds > 0
    End Sub
    Private Sub OpenpropToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenpropToolStripMenuItem.Click
        OpenFileDialog1.Filter = "Prop Files|*.prop|All Files|*.*"
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            RichTextBox1.Text = IO.File.ReadAllText(OpenFileDialog1.FileName)

        End If
    End Sub

    Private Sub SavepropToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SavepropToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Prop Files|*.prop|All Files|*.*"
        SaveFileDialog1.FileName = ""
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            IO.File.WriteAllText(SaveFileDialog1.FileName, RichTextBox1.Text)
        End If
    End Sub
    Public Sub Push_build()
        Dim myprocess As Process = New Process
        myprocess.StartInfo.FileName = "adb"
        myprocess.StartInfo.Arguments = "push " + SaveFileDialog1.FileName + " /sdcard/build.prop"
        myprocess.StartInfo.UseShellExecute = False
        myprocess.StartInfo.CreateNoWindow = True
        myprocess.StartInfo.RedirectStandardInput = True
        myprocess.StartInfo.RedirectStandardOutput = True
        myprocess.StartInfo.RedirectStandardError = True
        myprocess.Start()
        While (myprocess.HasExited = False)
            Dim sLine As String = myprocess.StandardOutput.ReadLine
            If (Not String.IsNullOrEmpty(sLine)) Then
            End If
            RichTextBox1.AppendText(sLine & vbCrLf)
            Application.DoEvents()
        End While
    End Sub
    Public Sub Pull_Build()
        Dim myprocess As Process = New Process
        myprocess.StartInfo.FileName = "adb"
        myprocess.StartInfo.Arguments = "pull /system/build.prop " + nsaveoutput
        myprocess.StartInfo.UseShellExecute = False
        myprocess.StartInfo.CreateNoWindow = True
        myprocess.StartInfo.RedirectStandardInput = True
        myprocess.StartInfo.RedirectStandardOutput = True
        myprocess.StartInfo.RedirectStandardError = True
        myprocess.Start()
        While (myprocess.HasExited = False)
            Dim sLine As String = myprocess.StandardOutput.ReadLine
            If (Not String.IsNullOrEmpty(sLine)) Then
            End If
            RichTextBox1.AppendText(sLine & vbCrLf)
            Application.DoEvents()
        End While
    End Sub
    Public nsaveoutput As String
    Public Sub Common_method()
        If IO.File.Exists("build.prop") Then
            IO.File.Delete("build.prop")
        End If
        SaveFileDialog1.Filter = "Prop files|*.prop|All Files|*.*"
        SaveFileDialog1.FileName = ""
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            nsaveoutput = SaveFileDialog1.FileName
        Else
            nsaveoutput = ""
        End If
        Pull_Build()
        Delay(1)
        If MsgBox("Do you want to Import it as an Project", vbYesNo) = vbYes Then
            If IO.File.Exists(nsaveoutput) Then
                RichTextBox1.Text = IO.File.ReadAllText(nsaveoutput)
            Else
                RichTextBox1.Text = IO.File.ReadAllText("build.prop")
            End If
        End If
    End Sub
    Private Sub PullFromDeviceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PullFromDeviceToolStripMenuItem.Click
        Phone = Adb.ExecuteAdbCommand(Adb.FormAdbCommand("get-state"))
        Select Case (Phone)
            Case "device"
                Common_method()
            Case "recovery"
                Common_method()
            Case "offline"
                MsgBox("Device Seems to be Offline")
            Case "unauthorized"
                MsgBox("Check your Device and Accept the upcoming box then try again")
            Case "unknown"
                MsgBox("Device may be not connected or in fastboot mode")
            Case "sideload"
                MsgBox("Can't do work in sideload mode")
            Case Else
                MsgBox("Device is not connected or in fastboot mode")
        End Select
    End Sub
    Public Phone, Rooted As String

    Public Sub SavefromResource(ByVal filepath As String, ByVal File As Object)
        Dim Fbyte() As Byte = File
        My.Computer.FileSystem.WriteAllBytes(filepath, Fbyte, True)
    End Sub

    Public Sub chmod()
        Dim myprocess As Process = New Process
        myprocess.StartInfo.FileName = "adb"
        myprocess.StartInfo.Arguments = "-d shell su -c chmod 644 /system/build.prop"
        myprocess.StartInfo.UseShellExecute = False
        myprocess.StartInfo.CreateNoWindow = True
        myprocess.StartInfo.RedirectStandardInput = True
        myprocess.StartInfo.RedirectStandardOutput = True
        myprocess.StartInfo.RedirectStandardError = True
        myprocess.Start()
        While (myprocess.HasExited = False)
            Dim sLine As String = myprocess.StandardOutput.ReadLine
            If (Not String.IsNullOrEmpty(sLine)) Then
            End If
            Application.DoEvents()
        End While
    End Sub
    Public Sub remo()
        Dim myprocess As Process = New Process
        myprocess.StartInfo.FileName = "adb"
        myprocess.StartInfo.Arguments = "-d shell su -c rm /sdcard/build.prop"
        myprocess.StartInfo.UseShellExecute = False
        myprocess.StartInfo.CreateNoWindow = True
        myprocess.StartInfo.RedirectStandardInput = True
        myprocess.StartInfo.RedirectStandardOutput = True
        myprocess.StartInfo.RedirectStandardError = True
        myprocess.Start()
        While (myprocess.HasExited = False)
            Dim sLine As String = myprocess.StandardOutput.ReadLine
            If (Not String.IsNullOrEmpty(sLine)) Then
            End If
            Application.DoEvents()
        End While
    End Sub
    Public Sub mv()
        Dim myprocess As Process = New Process
        myprocess.StartInfo.FileName = "adb"
        myprocess.StartInfo.Arguments = "-d shell su -c mv /sdcard/build.prop /system"
        myprocess.StartInfo.UseShellExecute = False
        myprocess.StartInfo.CreateNoWindow = True
        myprocess.StartInfo.RedirectStandardInput = True
        myprocess.StartInfo.RedirectStandardOutput = True
        myprocess.StartInfo.RedirectStandardError = True
        myprocess.Start()
        While (myprocess.HasExited = False)
            Dim sLine As String = myprocess.StandardOutput.ReadLine
            If (Not String.IsNullOrEmpty(sLine)) Then
            End If
            Application.DoEvents()
        End While
    End Sub
    Public Sub mount()
        Dim myprocess As Process = New Process
        myprocess.StartInfo.FileName = "adb"
        myprocess.StartInfo.Arguments = "-d shell su -c mount -o remount rw /system"
        myprocess.StartInfo.UseShellExecute = False
        myprocess.StartInfo.CreateNoWindow = True
        myprocess.StartInfo.RedirectStandardInput = True
        myprocess.StartInfo.RedirectStandardOutput = True
        myprocess.StartInfo.RedirectStandardError = True
        myprocess.Start()
        While (myprocess.HasExited = False)
            Dim sLine As String = myprocess.StandardOutput.ReadLine
            If (Not String.IsNullOrEmpty(sLine)) Then
            End If

            Application.DoEvents()
        End While
    End Sub
    Public Sub rm()
        Dim myprocess As Process = New Process
        myprocess.StartInfo.FileName = "adb"
        myprocess.StartInfo.Arguments = "-d shell su -c rm /system/build.prop"
        myprocess.StartInfo.UseShellExecute = False
        myprocess.StartInfo.CreateNoWindow = True
        myprocess.StartInfo.RedirectStandardInput = True
        myprocess.StartInfo.RedirectStandardOutput = True
        myprocess.StartInfo.RedirectStandardError = True
        myprocess.Start()
        While (myprocess.HasExited = False)
            Dim sLine As String = myprocess.StandardOutput.ReadLine
            If (Not String.IsNullOrEmpty(sLine)) Then
            End If
            Application.DoEvents()
        End While
    End Sub
    Public Sub Common_method1()
        SaveFileDialog1.Filter = "Prop Files|*.prop|All Files|*.*"
        SaveFileDialog1.FileName = ""

        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            IO.File.WriteAllText(SaveFileDialog1.FileName, RichTextBox1.Text)
            MsgBox("Keep your Device On, Any onscreen messages may pop up!")
            RichTextBox1.Cursor = Cursors.WaitCursor
            Delay(1)
            Push_build()
            Delay(1)
            mount()
            Delay(1)
            rm()
            Delay(1)
            mv()
            Delay(1)
            chmod()
            remo()
            RichTextBox1.Cursor = Cursors.IBeam
            MsgBox("Prop Has been successfully placed in system!")
        End If
    End Sub
    Private Sub PushToDeviceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PushToDeviceToolStripMenuItem.Click
        Phone = Adb.ExecuteAdbCommand(Adb.FormAdbCommand("get-state"))
        Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("pull /system/xbin/su"))
        If My.Computer.FileSystem.FileExists("su") Then
            IO.File.Delete("su")
            Select Case (Phone)
                Case "device"
                    Common_method1()
                Case "recovery"
                    Common_method1()
                Case "offline"
                    MsgBox("Device Seems to be Offline")
                Case "unauthorized"
                    MsgBox("Check your Device and Accept the upcoming box then try again")
                Case "unknown"
                    MsgBox("Device may be not connected or in fastboot mode")
                Case "sideload"
                    MsgBox("Can't do work in sideload mode")
                Case Else
                    MsgBox("Device is not connected or in fastboot mode")
            End Select
        Else
            MsgBox("Your Device is not Rooted to Push Prop")
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        If MsgBox("Do you really want to exit", vbYesNo) = vbYes Then
            Me.Close()
        End If
    End Sub

    Private Sub FontToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FontToolStripMenuItem.Click
        Try
            Dim dlg As FontDialog = New FontDialog
            dlg.Font = RichTextBox1.Font
            If dlg.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                RichTextBox1.Font = dlg.Font
            End If
        Catch ex As Exception : End Try
    End Sub

    Private Sub SearchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchToolStripMenuItem.Click
        Dim a As String
        Dim b As String
        a = InputBox("Enter property to be searched")
        b = InStr(RichTextBox1.Text, a)
        If b Then
            RichTextBox1.Focus()
            RichTextBox1.SelectionStart = b - 1
            RichTextBox1.SelectionLength = Len(a)
        Else
            MsgBox("Property not found.")
        End If
    End Sub

    Private Sub UndoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoToolStripMenuItem.Click
        RichTextBox1.Undo()

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        About.Close()
        About.Show()
    End Sub

    Private Sub VisitWebpageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VisitWebpageToolStripMenuItem.Click
        Process.Start("http://developerkp.capstricks.net/")
    End Sub
    Public Mods As String

    Public Sub Check_mod()
        If RichTextBox1.Text.Contains(Mods) Then
            MsgBox("You already have this Tweak")
        Else
            RichTextBox1.AppendText(Environment.NewLine & Mods)
        End If
    End Sub
    Private Sub RamManagementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RamManagementToolStripMenuItem.Click
        Mods = "ro.HOME_APP_ADJ=1"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub SupportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupportToolStripMenuItem.Click
        Process.Start("http://developerkp.capstricks.net/index.php/support/")
    End Sub

    Private Sub ImproveAudioAndVideoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImproveAudioAndVideoToolStripMenuItem.Click
        'Part 1
        Mods = "ro.media.enc.jpeg.quality = 100"
        Check_mod()
        Delay(1)
        Mods = "ro.media.dec.jpeg.memcap = 8000000"
        Check_mod()
        Delay(1)
        Mods = "ro.media.enc.hprof.vid.bps = 8000000"
        Check_mod()
        Delay(1)
        Mods = "ro.media.capture.maxres = 8m"
        Check_mod()
        Delay(1)
        Mods = "ro.media.panorama.defres = 3264×1840"
        Check_mod()
        Delay(1)
        Mods = "ro.media.panorama.frameres = 1280×720"
        Check_mod()
        Delay(1)
        Mods = "ro.camcorder.videoModes = True"
        Check_mod()
        Delay(1)
        Mods = "ro.media.enc.hprof.vid.fps = 65"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub FasterStreamingVideosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FasterStreamingVideosToolStripMenuItem.Click
        Mods = "media.stagefright.enable-player=true"
        Check_mod()
        Delay(1)
        Mods = "media.stagefright.enable-meta=true"
        Check_mod()
        Delay(1)
        Mods = "media.stagefright.enable-scan=true"
        Check_mod()
        Delay(1)
        Mods = "media.stagefright.enable-http=true"
        Check_mod()
        Delay(1)
        Mods = "media.stagefright.enable-rtsp=true"
        Check_mod()
        Delay(1)
        Mods = "media.stagefright.enable-record=false"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub VideoAccelerationAndHWDebuggingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VideoAccelerationAndHWDebuggingToolStripMenuItem.Click
        Mods = "debug.sf.hw=1"
        Check_mod()
        Delay(1)
        Mods = "debug.performance.tuning=1"
        Check_mod()
        Delay(1)
        Mods = "video.accelerate.hw=1"
        Check_mod()
        Delay(1)
        Mods = "debug.egl.profiler=1"
        Check_mod()
        Delay(1)
        Mods = "debug.egl.hw=1"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub DisablesBuiltInErrorReportingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisablesBuiltInErrorReportingToolStripMenuItem.Click
        Mods = "profiler.force_disable_err_rpt=1"
        Check_mod()
        Delay(1)
        Mods = "profiler.force_disable_ulog=1"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub BetterNetSpeedsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BetterNetSpeedsToolStripMenuItem.Click
        Mods = "net.tcp.buffersize.default=4096,87380,256960, 4096, 16384,256960"
        Check_mod()
        Delay(1)
        Mods = "net.tcp.buffersize.wifi=4096,87380,256960,409 6,163 84,256960"
        Check_mod()
        Delay(1)
        Mods = "net.tcp.buffersize.umts=4096,8 7380,256960,4096,163 84,256960"
        Check_mod()
        Delay(1)
        Mods = "net.tcp.buffersize.gprs=4096,8 7380,256960,4096,163 84,256960"
        Check_mod()
        Delay(1)
        Mods = "net.tcp.buffersize.edge=4096,8 7380,256960,4096,163 84,256960"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub SavesPowerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SavesPowerToolStripMenuItem.Click
        Mods = "ro.mot.eri.losalert.delay=1000" 'May affect teathering
        Check_mod()
        Delay(1)
        Mods = "ro.ril.power_collapse=1"
        Check_mod()
        Delay(1)
        Mods = "pm.sleep_mode=1"
        Check_mod()
        Delay(1)
        Mods = "wifi.supplicant_scan_interval=180"
        Check_mod()
        Delay(1)
        Mods = "ro.mot.eri.losalert.delay=1000"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub GTweaksToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GTweaksToolStripMenuItem.Click
        Mods = "ro.ril.hep=0"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.hsxpa=2"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.gprsclass=12"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.enable.dtm=1"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.hsdpa.category=8"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.enable.a53=1"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.enable.3g.prefix=1"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.htcmaskw1.bitmask=4294967295"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.htcmaskw1=14449"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.hsupa.category=6"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub DisablesLogcatToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisablesLogcatToolStripMenuItem.Click
        Mods = "logcat.live=disable"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub PhoneRingsImmediatelyDuringCallToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PhoneRingsImmediatelyDuringCallToolStripMenuItem.Click
        Mods = "ro.telephony.call_ring.delay=0"
        Check_mod()
        Delay(1)
        Mods = "ring.delay=0"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub DisablesBlackscreenIssueAfterACallToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisablesBlackscreenIssueAfterACallToolStripMenuItem.Click
        Mods = "ro.lge.proximity.delay=25"
        Check_mod()
        Delay(1)
        Mods = "mot.proximity.delay=25"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub BetterScrollingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BetterScrollingToolStripMenuItem.Click
        Mods = "windowsmgr.max_events_per_sec=150"
        Check_mod()
        Delay(1)
        Mods = "ro.min_pointer_dur=8 ro.max.fling_velocity=12000"
        Check_mod()
        Delay(1)
        Mods = "ro.min.fling_velocity=8000"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub BetterSignalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BetterSignalToolStripMenuItem.Click
        Mods = "persist.cust.tel.eons=1"
        Check_mod()
        Mods = "ro.config.hw_fast_dormancy=1"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub BetterCallVoiceQualityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BetterCallVoiceQualityToolStripMenuItem.Click
        Mods = "ro.ril.enable.amr.wideband=1"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub FasterBootToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FasterBootToolStripMenuItem.Click
        Mods = "ro.config.hw_quickpoweron=true"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub DisablesErrorCheckingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisablesErrorCheckingToolStripMenuItem.Click
        Mods = "ro.kernel.android.checkjni=0"
        Check_mod()
        Delay(1)
        Mods = "ro.kernel.checkjni=0"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub DalvikVirtualMachineTweaksToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DalvikVirtualMachineTweaksToolStripMenuItem.Click
        Mods = "dalvik.vm.checkjni=false"
        Check_mod()
        Delay(1)
        Mods = "dalvik.vm.dexopt-data-only=1"
        Check_mod()
        Delay(1)
        Mods = "dalvik.vm.heapstartsize=5m"
        Check_mod()
        Delay(1)
        Mods = "dalvik.vm.heapgrowthlimit=48m"
        Check_mod()
        Delay(1)
        Mods = "dalvik.vm.heapsize=64m"
        Check_mod()
        Delay(1)
        Mods = "dalvik.vm.verify-bytecode=false"
        Check_mod()
        Delay(1)
        Mods = "dalvik.vm.execution-mode=int:jit"
        Check_mod()
        Delay(1)
        Mods = "dalvik.vm.lockprof.threshold=250"
        Check_mod()
        Delay(1)
        Mods = "dalvik.vm.dexopt-flags=m=v,o=y"
        Check_mod()
        Delay(1)
        Mods = "dalvik.vm.stack-trace-file=/data/anr/traces.txt"
        Check_mod()
        Delay(1)
        Mods = "dalvik.vm.jmiopts=forcecopy"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub DisableNotificationWhileAdbIsActiveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisableNotificationWhileAdbIsActiveToolStripMenuItem.Click
        Mods = "persist.adb.notify=0"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub BetterBatteryLifeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BetterBatteryLifeToolStripMenuItem.Click
        Mods = "wifi.supplicant_scan_interval=180"
        Check_mod()
        Delay(1)
        Mods = "pm.sleep_mode=1"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.disable.power.collapse=0"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub ImprovePerformanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImprovePerformanceToolStripMenuItem.Click
        Mods = "debug.performance.tuning=1"
        Check_mod()
        MsgBox("Tweak Integrated")
    End Sub

    Private Sub AdbRebootToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdbRebootToolStripMenuItem.Click
        Adb.ExecuteAdbCommand(Adb.FormAdbCommand("reboot"))
    End Sub
    Public Sub zadd()
        Dim myprocess As Process = New Process
        myprocess.StartInfo.FileName = "7z"
        myprocess.StartInfo.Arguments = "u " + SaveFileDialog1.FileName + " system"
        myprocess.StartInfo.UseShellExecute = False
        myprocess.StartInfo.CreateNoWindow = True
        myprocess.StartInfo.RedirectStandardInput = True
        myprocess.StartInfo.RedirectStandardOutput = True
        myprocess.StartInfo.RedirectStandardError = True
        myprocess.Start()
        While (myprocess.HasExited = False)
            Dim sLine As String = myprocess.StandardOutput.ReadLine
            If (Not String.IsNullOrEmpty(sLine)) Then
            End If
            Application.DoEvents()
        End While
    End Sub
    Private Sub MakeFlashableZipToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MakeFlashableZipToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Zip Files|*.zip|All Files|*.*"
        SaveFileDialog1.FileName = ""
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            SavefromResource(SaveFileDialog1.FileName, My.Resources.demo)
            My.Computer.FileSystem.CreateDirectory("system")
            IO.File.WriteAllText("system\build.prop", RichTextBox1.Text)
            Delay(1)
            zadd()
            Delay(1)

            MsgBox("File Saved at " + SaveFileDialog1.FileName)
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not My.Computer.FileSystem.FileExists("7z.exe") Then
            SavefromResource("7z.exe", My.Resources._7za)
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        android.UpdateDeviceList()
        If (android.HasConnectedDevices) Then
            Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("pull /system/xbin/su"))
            Phone = Adb.ExecuteAdbCommand(Adb.FormAdbCommand("get-state"))
            If My.Computer.FileSystem.FileExists("su") Then
                IO.File.Delete("su")
                Rooted = "True"
            Else
                Rooted = "False"
            End If
        End If
    End Sub
End Class
