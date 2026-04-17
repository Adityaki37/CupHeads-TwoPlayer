Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006DC RID: 1756
Public Class MouseLevelBackgroundHopper
	Inherits AbstractMonoBehaviour

	' Token: 0x06002565 RID: 9573 RVA: 0x0015D9DB File Offset: 0x0015BDDB
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.startPos = MyBase.transform.localPosition
	End Sub

	' Token: 0x06002566 RID: 9574 RVA: 0x0015D9F9 File Offset: 0x0015BDF9
	Private Sub Start()
		AddHandler CupheadLevelCamera.Current.OnShakeEvent, AddressOf Me.OnShake
	End Sub

	' Token: 0x06002567 RID: 9575 RVA: 0x0015DA11 File Offset: 0x0015BE11
	Private Sub OnDestroy()
		If CupheadLevelCamera.Current IsNot Nothing Then
			Me.RemoveShake()
		End If
	End Sub

	' Token: 0x06002568 RID: 9576 RVA: 0x0015DA29 File Offset: 0x0015BE29
	Private Sub RemoveShake()
		RemoveHandler CupheadLevelCamera.Current.OnShakeEvent, AddressOf Me.OnShake
	End Sub

	' Token: 0x06002569 RID: 9577 RVA: 0x0015DA41 File Offset: 0x0015BE41
	Private Sub OnShake(amount As Single, time As Single)
		If Me.hopCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.hopCoroutine)
		End If
		Me.hopCoroutine = MyBase.StartCoroutine(Me.hop_cr(amount))
	End Sub

	' Token: 0x0600256A RID: 9578 RVA: 0x0015DA70 File Offset: 0x0015BE70
	Private Iterator Function hop_cr(amount As Single) As IEnumerator
		For i As Integer = 0 To Me.hops.Length - 1
			Dim height As Single = Me.hops(i).height
			Dim time As Single = Me.hops(i).time
			Dim endPos As Vector2 = Me.startPos + New Vector2(0F, height)
			Dim ht As Single = time / 2F
			Yield MyBase.StartCoroutine(Me.tween_cr(Me.startPos.y, endPos.y, ht, EaseUtils.EaseType.easeOutSine))
			Yield MyBase.StartCoroutine(Me.tween_cr(endPos.y, Me.startPos.y, ht, EaseUtils.EaseType.easeInSine))
			time /= 2F
			height /= 2F
		Next
		Return
	End Function

	' Token: 0x0600256B RID: 9579 RVA: 0x0015DA8C File Offset: 0x0015BE8C
	Private Iterator Function tween_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		MyBase.transform.SetLocalPosition(New Single?(Me.startPos.x), New Single?(start), New Single?(0F))
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			MyBase.transform.SetLocalPosition(New Single?(Me.startPos.x), New Single?(EaseUtils.Ease(ease, start, [end], val)), New Single?(0F))
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetLocalPosition(New Single?(Me.startPos.x), New Single?([end]), New Single?(0F))
		Return
	End Function

	' Token: 0x04002DF2 RID: 11762
	Public hops As MouseLevelBackgroundHopper.Hop() = New MouseLevelBackgroundHopper.Hop(0) {}

	' Token: 0x04002DF3 RID: 11763
	Private hopCoroutine As Coroutine

	' Token: 0x04002DF4 RID: 11764
	Private startPos As Vector2

	' Token: 0x020006DD RID: 1757
	<Serializable()>
	Public Class Hop
		' Token: 0x04002DF5 RID: 11765
		Public height As Single = 50F

		' Token: 0x04002DF6 RID: 11766
		Public time As Single = 0.25F
	End Class
End Class
