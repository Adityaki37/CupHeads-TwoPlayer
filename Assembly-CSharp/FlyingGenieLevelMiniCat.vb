Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000670 RID: 1648
Public Class FlyingGenieLevelMiniCat
	Inherits HomingProjectile

	' Token: 0x17000396 RID: 918
	' (get) Token: 0x060022A8 RID: 8872 RVA: 0x00145A51 File Offset: 0x00143E51
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x060022A9 RID: 8873 RVA: 0x00145A54 File Offset: 0x00143E54
	Public Function Create(pos As Vector3, rotation As Single, player As AbstractPlayerController, properties As LevelProperties.FlyingGenie.Sphinx) As FlyingGenieLevelMiniCat
		Dim flyingGenieLevelMiniCat As FlyingGenieLevelMiniCat = TryCast(MyBase.Create(pos, rotation, properties.homingSpeed, properties.homingSpeed, properties.homingRotation, 20F, 0F, player), FlyingGenieLevelMiniCat)
		flyingGenieLevelMiniCat.properties = properties
		flyingGenieLevelMiniCat.transform.position = pos
		Return flyingGenieLevelMiniCat
	End Function

	' Token: 0x060022AA RID: 8874 RVA: 0x00145AA9 File Offset: 0x00143EA9
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.timer_cr())
	End Sub

	' Token: 0x060022AB RID: 8875 RVA: 0x00145ABE File Offset: 0x00143EBE
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.properties.dieOnCollisionPlayer Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060022AC RID: 8876 RVA: 0x00145ADE File Offset: 0x00143EDE
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060022AD RID: 8877 RVA: 0x00145AFC File Offset: 0x00143EFC
	Private Iterator Function timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.miniHomingDurationRange.RandomFloat())
		MyBase.HomingEnabled = False
		While True
			MyBase.transform.position += MyBase.transform.right * Me.properties.homingSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060022AE RID: 8878 RVA: 0x00145B17 File Offset: 0x00143F17
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		MyBase.animator.SetFloat("Pink", CSng(If((Not parryable), 0, 1)))
	End Sub

	' Token: 0x04002B4C RID: 11084
	Private Const PinkParameterName As String = "Pink"

	' Token: 0x04002B4D RID: 11085
	Private properties As LevelProperties.FlyingGenie.Sphinx
End Class
