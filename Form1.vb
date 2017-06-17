Imports System
Imports RegawMOD.Android
Imports System.IO
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports FastColoredTextBoxNS
Public Class Form1
    Dim android As AndroidController
    Dim device As Device
    Public path
    Dim BrownStyle = New TextStyle(Brushes.Brown, Nothing, FontStyle.Regular)
    Dim GreyStyle = New TextStyle(Brushes.Gray, Nothing, FontStyle.Italic)
    Dim KeywordsStyle = New TextStyle(Brushes.Green, Nothing, FontStyle.Regular)
    Dim FunctionNameStyle = New TextStyle(Brushes.Blue, Nothing, FontStyle.Regular)
    Public Sub Delay(ByVal DelayInSeconds As Integer)
        Dim ts As TimeSpan
        Dim targetTime As DateTime = DateTime.Now.AddSeconds(DelayInSeconds)
        Do
            ts = targetTime.Subtract(DateTime.Now)
            Application.DoEvents() ' keep app responsive
            System.Threading.Thread.Sleep(50) ' reduce CPU usage
        Loop While ts.TotalSeconds > 0
    End Sub
    Private Sub OpenpropToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenpropToolStripMenuItem.Click, ToolStripButton1.Click
        OpenFileDialog1.Filter = "Prop Files|*.prop|All Files|*.*"
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            RichTextBox2.Text = IO.File.ReadAllText(OpenFileDialog1.FileName)
            path = OpenFileDialog1.FileName
            SavepropToolStripMenuItem.Image = My.Resources.save
            ToolStripButton2.Image = My.Resources.save
        End If
    End Sub

    Private Sub SavepropToolStripMenuItem_Click(sender As Object, e As EventArgs)

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
            RichTextBox2.AppendText(sLine & vbCrLf)
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
            RichTextBox2.AppendText(sLine & vbCrLf)
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
                RichTextBox2.Text = IO.File.ReadAllText(nsaveoutput)
            Else
                RichTextBox2.Text = IO.File.ReadAllText("build.prop")
            End If
        End If
    End Sub
    Private Sub PullFromDeviceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripButton8.Click
        Phone = Adb.ExecuteAdbCommand(Adb.FormAdbCommand("get-state"))
        Select Case (Phone)
            Case "device"
                Common_method()
            Case "recovery"
                Common_method()
            Case "offline"
                Label1.Text = "Device Seems to be Offline"
            Case "unauthorized"
                Label1.Text = "Check your Device and Accept the upcoming box then try again"
            Case "unknown"
                Label1.Text = "Device may be not connected or in fastboot mode"
            Case "sideload"
                Label1.Text = "Can't do work in sideload mode"
            Case Else
                Label1.Text = "Device is not connected or in fastboot mode"
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
            IO.File.WriteAllText(SaveFileDialog1.FileName, RichTextBox2.Text)
            MsgBox("Keep your Device On, Any onscreen messages may pop up!")
            RichTextBox2.Cursor = Cursors.WaitCursor
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
            RichTextBox2.Cursor = Cursors.IBeam
            Label1.Text = "SuccessFully Pushed!"
        End If
    End Sub
    Private Sub PushToDeviceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripButton7.Click
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
                    Label1.Text = "Device Seems to be Offline"
                Case "unauthorized"
                    Label1.Text = "Check your Device and Accept the upcoming box then try again"
                Case "unknown"
                    Label1.Text = "Device may be not connected or in fastboot mode"
                Case "sideload"
                    Label1.Text = "Can't do work in sideload mode"
                Case Else
                    Label1.Text = "Device is not connected or in fastboot mode"
            End Select
        Else
            Label1.Text = "Error ! Device is not Rooted"
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click, ToolStripButton13.Click
        Me.Close()
    End Sub

    Private Sub FontToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Try
            Dim dlg As FontDialog = New FontDialog
            dlg.Font = RichTextBox2.Font
            If dlg.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                RichTextBox2.Font = dlg.Font
            End If
        Catch ex As Exception : End Try
    End Sub

    Private Sub SearchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchToolStripMenuItem.Click, ToolStripButton5.Click
        RichTextBox2.ShowFindDialog()

    End Sub

    Private Sub UndoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripButton9.Click
        RichTextBox2.Undo()

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click, ToolStripButton11.Click
        About.Close()
        About.ShowDialog()
    End Sub

    Private Sub VisitWebpageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VisitWebpageToolStripMenuItem.Click, ToolStripButton12.Click
        Process.Start("http://developerkp.capstricks.net/index.php/2017/04/12/prop-tweaker/")
    End Sub
    Public Mods As String

    Public Sub Check_mod()
        Timer2.Stop()

        If RichTextBox2.Text.Contains(Mods) Then
            Label1.Text = "You already have this Tweak"
        Else
            RichTextBox2.AppendText(Environment.NewLine & Mods)
        End If
    End Sub
    Private Sub RamManagementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RamManagementToolStripMenuItem.Click
        Mods = "# Improve Ram Management"
        Check_mod()
        Mods = "ro.HOME_APP_ADJ=1"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub SupportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupportToolStripMenuItem.Click
        Process.Start("http://developerkp.capstricks.net/index.php/support/")
    End Sub

    Private Sub ImproveAudioAndVideoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImproveAudioAndVideoToolStripMenuItem.Click
        'Part 1
        Mods = "# Improve Audio and Video"
        Check_mod()
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
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub FasterStreamingVideosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FasterStreamingVideosToolStripMenuItem.Click
        Mods = "# Faster Streaming Videos"
        Check_mod()
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
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub VideoAccelerationAndHWDebuggingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VideoAccelerationAndHWDebuggingToolStripMenuItem.Click
        Mods = "# Video Acceleration And HW Debugging"
        Check_mod()
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
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub DisablesBuiltInErrorReportingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisablesBuiltInErrorReportingToolStripMenuItem.Click
        Mods = "# Disables Built In Error Reporting"
        Check_mod()
        Mods = "profiler.force_disable_err_rpt=1"
        Check_mod()
        Delay(1)
        Mods = "profiler.force_disable_ulog=1"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub BetterNetSpeedsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BetterNetSpeedsToolStripMenuItem.Click
        Mods = "# Better Net Speeds"
        Check_mod()
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
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub SavesPowerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SavesPowerToolStripMenuItem.Click
        Mods = "# Saves Power"
        Check_mod()
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
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub GTweaksToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GTweaksToolStripMenuItem.Click
        Mods = "# 3G Tweaks"
        Check_mod()
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
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub DisablesLogcatToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisablesLogcatToolStripMenuItem.Click
        Mods = "# Disables Logcat"
        Check_mod()
        Mods = "logcat.live=disable"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub PhoneRingsImmediatelyDuringCallToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PhoneRingsImmediatelyDuringCallToolStripMenuItem.Click
        Mods = "# Phone Rings Immediately During Call"
        Check_mod()
        Mods = "ro.telephony.call_ring.delay=0"
        Check_mod()
        Delay(1)
        Mods = "ring.delay=0"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub DisablesBlackscreenIssueAfterACallToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisablesBlackscreenIssueAfterACallToolStripMenuItem.Click
        Mods = "# Disables Blackscreen Issue After A Call"
        Check_mod()
        Mods = "ro.lge.proximity.delay=25"
        Check_mod()
        Delay(1)
        Mods = "mot.proximity.delay=25"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub BetterScrollingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BetterScrollingToolStripMenuItem.Click
        Mods = "# Better Scrolling"
        Check_mod()
        Mods = "windowsmgr.max_events_per_sec=150"
        Check_mod()
        Delay(1)
        Mods = "ro.min_pointer_dur=8 ro.max.fling_velocity=12000"
        Check_mod()
        Delay(1)
        Mods = "ro.min.fling_velocity=8000"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub BetterSignalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BetterSignalToolStripMenuItem.Click
        Mods = "# Better Signal"
        Check_mod()
        Mods = "persist.cust.tel.eons=1"
        Check_mod()
        Mods = "ro.config.hw_fast_dormancy=1"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub BetterCallVoiceQualityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BetterCallVoiceQualityToolStripMenuItem.Click
        Mods = "# Better Call Voice Quality"
        Check_mod()
        Mods = "ro.ril.enable.amr.wideband=1"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub FasterBootToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FasterBootToolStripMenuItem.Click
        Mods = "# Faster Boot"
        Check_mod()
        Mods = "ro.config.hw_quickpoweron=true"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub DisablesErrorCheckingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisablesErrorCheckingToolStripMenuItem.Click
        Mods = "# Disables Error Checking"
        Check_mod()
        Mods = "ro.kernel.android.checkjni=0"
        Check_mod()
        Delay(1)
        Mods = "ro.kernel.checkjni=0"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub DalvikVirtualMachineTweaksToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DalvikVirtualMachineTweaksToolStripMenuItem.Click
        Mods = "# Dalvik Virtual Machine Tweaks"
        Check_mod()
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
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub DisableNotificationWhileAdbIsActiveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisableNotificationWhileAdbIsActiveToolStripMenuItem.Click
        Mods = "# Disable Notification While AdbIsActive"
        Check_mod()
        Mods = "persist.adb.notify=0"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub BetterBatteryLifeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BetterBatteryLifeToolStripMenuItem.Click
        Mods = "# Better Battery Life"
        Check_mod()
        Mods = "wifi.supplicant_scan_interval=180"
        Check_mod()
        Delay(1)
        Mods = "pm.sleep_mode=1"
        Check_mod()
        Delay(1)
        Mods = "ro.ril.disable.power.collapse=0"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
    End Sub

    Private Sub ImprovePerformanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImprovePerformanceToolStripMenuItem.Click
        Mods = "# Improve Performance"
        Check_mod()
        Mods = "debug.performance.tuning=1"
        Check_mod()
        Label1.Text = "Tweak Integrated in last part"
        Timer2.Start()
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
    Private Sub MakeFlashableZipToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MakeFlashableZipToolStripMenuItem.Click, ToolStripButton4.Click
        SaveFileDialog1.Filter = "Zip Files|*.zip|All Files|*.*"
        SaveFileDialog1.FileName = ""
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            SavefromResource(SaveFileDialog1.FileName, My.Resources.demo)
            My.Computer.FileSystem.CreateDirectory("system")
            IO.File.WriteAllText("system\build.prop", RichTextBox2.Text)
            Delay(1)
            zadd()
            Label1.Text = "File Saved"
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer2.Start()

        If Not My.Computer.FileSystem.FileExists("7z.exe") Then
            SavefromResource("7z.exe", My.Resources._7za)
        End If
        If Not My.Computer.FileSystem.FileExists("7z.dll") Then
            SavefromResource("7z.dll", My.Resources._7z)
        End If

        SavepropToolStripMenuItem.Image = My.Resources.unsave
        ToolStripButton2.Image = My.Resources.unsave
    End Sub

    Private Sub PullFromDeviceToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles PullFromDeviceToolStripMenuItem.Click
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

    Private Sub PushToDeviceToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles PushToDeviceToolStripMenuItem.Click
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

    Private Sub UndoToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles UndoToolStripMenuItem.Click
        RichTextBox2.Undo()
    End Sub

    Private Sub ReplaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReplaceToolStripMenuItem.Click, ToolStripButton6.Click
        RichTextBox2.ShowReplaceDialog()
    End Sub

    Private Sub RedoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RedoToolStripMenuItem.Click, ToolStripButton10.Click
        RichTextBox2.Redo()
    End Sub

    Private Sub RichTextBox2_Load(sender As Object, e As EventArgs) Handles RichTextBox2.Load

    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click, SavepropToolStripMenuItem.Click

        If path = Nothing Then
            SaveFileDialog1.Filter = "Prop Files|*.prop|All Files|*.*"
            SaveFileDialog1.FileName = ""
            If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                path = SaveFileDialog1.FileName
                IO.File.WriteAllText(SaveFileDialog1.FileName, RichTextBox2.Text)
                SavepropToolStripMenuItem.Image = My.Resources.save
                ToolStripButton2.Image = My.Resources.save
            End If
        Else
            SavepropToolStripMenuItem.Image = My.Resources.save
            ToolStripButton2.Image = My.Resources.save
            IO.File.WriteAllText(path, RichTextBox2.Text)
        End If
    End Sub

    Private Sub RichTextBox2_TextChanging(sender As Object, e As FastColoredTextBoxNS.TextChangingEventArgs) Handles RichTextBox2.TextChanging
        SavepropToolStripMenuItem.Image = My.Resources.unsave
        ToolStripButton2.Image = My.Resources.unsave
    End Sub

    Private Sub RichTextBox2_TextChangedDelayed(sender As Object, e As TextChangedEventArgs) Handles RichTextBox2.TextChangedDelayed
        e.ChangedRange.ClearStyle(GreyStyle)
        RichTextBox2.Range.ClearStyle(KeywordsStyle, FunctionNameStyle)
        e.ChangedRange.SetStyle(GreyStyle, "#.*$", RegexOptions.Multiline)
        RichTextBox2.Range.SetStyle(KeywordsStyle, "\b(and|eval|else|if|lambda|or|set|ro|defun)\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        RichTextBox2.Range.SetStyle(FunctionNameStyle, "\b(dalvik|persist|ril)\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        RichTextBox2.Range.SetStyle(BrownStyle, "\b(mediatek|fmradio|drm|debug|rild|wifi|bgw|wfd|net)\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click, ToolStripButton3.Click
        SaveFileDialog1.Filter = "Prop Files|*.prop|All Files|*.*"
        SaveFileDialog1.FileName = ""
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            path = SaveFileDialog1.FileName
            IO.File.WriteAllText(SaveFileDialog1.FileName, RichTextBox2.Text)
            SavepropToolStripMenuItem.Image = My.Resources.save
            ToolStripButton2.Image = My.Resources.save
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Label1.Text = "Ready"
    End Sub

    Private Sub GoToToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GoToToolStripMenuItem.Click
        RichTextBox2.ShowGoToDialog()
    End Sub

    Private Sub AdbRebootRecoveryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdbRebootRecoveryToolStripMenuItem.Click, AboutToolStripMenuItem.Click
        Adb.ExecuteAdbCommand(Adb.FormAdbCommand("reboot recovery"))
    End Sub

    Private Sub ShowConnectedDeviceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowConnectedDeviceToolStripMenuItem.Click
        Dim serial = Adb.ExecuteAdbCommand(Adb.FormAdbCommand("devices"))
        MsgBox(serial)
    End Sub

    Private Sub GithubSourceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GithubSourceToolStripMenuItem.Click
        Process.Start("https://github.com/KaustubhPatange/Build-Prop-Tweaker")
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not RichTextBox2.Text = Nothing Then
            If path = Nothing Then
                Dim var = MsgBox("Do you Want to save Prop ?", vbYesNo)
                If var = MsgBoxResult.Yes Then
                    SaveFileDialog1.Filter = "Prop Files|*.prop|All Files|*.*"
                    SaveFileDialog1.FileName = ""
                    If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                        path = SaveFileDialog1.FileName
                        IO.File.WriteAllText(SaveFileDialog1.FileName, RichTextBox2.Text)
                        SavepropToolStripMenuItem.Image = My.Resources.save
                        ToolStripButton2.Image = My.Resources.save
                    End If
                Else
                    SavepropToolStripMenuItem.Image = My.Resources.save
                    ToolStripButton2.Image = My.Resources.save
                    IO.File.WriteAllText(path, RichTextBox2.Text)
                End If
            Else
                Dim var = MsgBox("Do you Want to save Prop ?", vbYesNo)
                If var = MsgBoxResult.Yes Then
                    SavepropToolStripMenuItem.Image = My.Resources.save
                    ToolStripButton2.Image = My.Resources.save
                    IO.File.WriteAllText(path, RichTextBox2.Text)
                End If
            End If
        End If
    End Sub

    Private Sub RichTextBox2_Enter(sender As Object, e As EventArgs) Handles RichTextBox2.Enter

    End Sub

    Private Sub RichTextBox2_DragEnter(sender As Object, e As DragEventArgs) Handles RichTextBox2.DragEnter

    End Sub
    Public Sub Invoker()
        SaveFileDialog1.Filter = "Prop Files|*.prop|All Files|*.*"
        SaveFileDialog1.FileName = ""
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            path = SaveFileDialog1.FileName
            IO.File.WriteAllText(SaveFileDialog1.FileName, RichTextBox2.Text)
            SavepropToolStripMenuItem.Image = My.Resources.save
            ToolStripButton2.Image = My.Resources.save
        End If
    End Sub

    Private Sub RichTextBox2_DragDrop(sender As Object, e As DragEventArgs) Handles RichTextBox2.DragDrop

    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter

    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop

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
