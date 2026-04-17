Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000730 RID: 1840
Public Class PirateLevelPirateDead
	Inherits AbstractMonoBehaviour

	' Token: 0x0600281F RID: 10271 RVA: 0x0017690D File Offset: 0x00174D0D
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x06002820 RID: 10272 RVA: 0x00176921 File Offset: 0x00174D21
	Public Sub Go(delay As Single, speed As Single)
		MyBase.gameObject.SetActive(True)
		MyBase.StartCoroutine(Me.go_cr(delay, speed))
	End Sub

	' Token: 0x06002821 RID: 10273 RVA: 0x0017693E File Offset: 0x00174D3E
	Private Sub [End]()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002822 RID: 10274 RVA: 0x00176954 File Offset: 0x00174D54
	Private Iterator Function go_cr(delay As Single, time As Single) As IEnumerator
		Dim startY As Single = MyBase.transform.position.y
		Dim splash As Boolean = False
		Yield CupheadTime.WaitForSeconds(Me, delay)
		Dim t As Single = 0F
		While t < time
			Dim y As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, startY, -250F, t / time)
			MyBase.transform.SetLocalPosition(Nothing, New Single?(y), Nothing)
			If Not splash AndAlso MyBase.transform.position.y <= -25F Then
				splash = True
				Me.splashPrefab.Create(MyBase.transform.position + New Vector3(0F, 20F, 0F))
			End If
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetLocalPosition(Nothing, New Single?(-250F), Nothing)
		Me.[End]()
		Return
	End Function

	' Token: 0x06002823 RID: 10275 RVA: 0x0017697D File Offset: 0x00174D7D
	Private Sub OnDestroy()
		Me.splashPrefab = Nothing
	End Sub

	' Token: 0x040030DF RID: 12511
	Public Const END_Y As Single = -250F

	' Token: 0x040030E0 RID: 12512
	Public Const SPLASH_Y As Single = -25F

	' Token: 0x040030E1 RID: 12513
	<SerializeField()>
	Private splashPrefab As Effect
End Class
