Imports System
Imports UnityEngine

' Token: 0x02000511 RID: 1297
Public Class BeeLevelBackgroundGroup
	Inherits AbstractMonoBehaviour

	' Token: 0x06001714 RID: 5908 RVA: 0x000CF8D8 File Offset: 0x000CDCD8
	Private Sub Start()
		Me.level = TryCast(Level.Current, BeeLevel)
	End Sub

	' Token: 0x06001715 RID: 5909 RVA: 0x000CF8EC File Offset: 0x000CDCEC
	Private Sub Update()
		If MyBase.transform.localPosition.y < -800F Then
			Me.SetY(MyBase.transform.localPosition.y + CSng(Me.count) * 455F)
			Me.Randomize()
		End If
	End Sub

	' Token: 0x06001716 RID: 5910 RVA: 0x000CF944 File Offset: 0x000CDD44
	Private Sub FixedUpdate()
		Me.SetY(MyBase.transform.localPosition.y + Me.level.Speed * CupheadTime.Delta)
	End Sub

	' Token: 0x06001717 RID: 5911 RVA: 0x000CF984 File Offset: 0x000CDD84
	Public Sub Init(platforms As BeeLevelPlatforms, groupCount As Integer)
		Me.level = TryCast(Level.Current, BeeLevel)
		Me.count = groupCount
		Me.platforms = Global.UnityEngine.[Object].Instantiate(Of BeeLevelPlatforms)(platforms)
		Me.platforms.transform.SetParent(MyBase.transform)
		Me.platforms.Init()
		Me.Randomize()
	End Sub

	' Token: 0x06001718 RID: 5912 RVA: 0x000CF9DB File Offset: 0x000CDDDB
	Public Sub Randomize()
		Me.DisableAll()
		Me.platforms.Randomize(Me.level.MissingPlatformCount)
		Me.variations(Global.UnityEngine.Random.Range(0, Me.variations.Length)).SetActive(True)
	End Sub

	' Token: 0x06001719 RID: 5913 RVA: 0x000CFA14 File Offset: 0x000CDE14
	Private Sub DisableAll()
		For Each gameObject As GameObject In Me.variations
			gameObject.SetActive(False)
		Next
	End Sub

	' Token: 0x0600171A RID: 5914 RVA: 0x000CFA47 File Offset: 0x000CDE47
	Public Sub SetY(y As Single)
		MyBase.transform.SetPosition(New Single?(0F), New Single?(y), New Single?(0F))
	End Sub

	' Token: 0x0400205C RID: 8284
	Private Const MIN_Y As Single = -800F

	' Token: 0x0400205D RID: 8285
	<SerializeField()>
	Private variations As GameObject()

	' Token: 0x0400205E RID: 8286
	Private level As BeeLevel

	' Token: 0x0400205F RID: 8287
	Private platforms As BeeLevelPlatforms

	' Token: 0x04002060 RID: 8288
	Private count As Integer

	' Token: 0x04002061 RID: 8289
	Private lastCount As Integer
End Class
