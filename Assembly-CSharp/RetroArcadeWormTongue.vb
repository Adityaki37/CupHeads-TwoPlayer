Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200076E RID: 1902
Public Class RetroArcadeWormTongue
	Inherits AbstractCollidableObject

	' Token: 0x170003E6 RID: 998
	' (get) Token: 0x0600295C RID: 10588 RVA: 0x00181D12 File Offset: 0x00180112
	' (set) Token: 0x0600295D RID: 10589 RVA: 0x00181D1A File Offset: 0x0018011A
	Public Property TileRotationSpeed As Single

	' Token: 0x0600295E RID: 10590 RVA: 0x00181D23 File Offset: 0x00180123
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.GetComponentInChildren(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x0600295F RID: 10591 RVA: 0x00181D42 File Offset: 0x00180142
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002960 RID: 10592 RVA: 0x00181D5A File Offset: 0x0018015A
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06002961 RID: 10593 RVA: 0x00181D71 File Offset: 0x00180171
	Public Sub Init(properties As LevelProperties.RetroArcade.Worm)
		Me.properties = properties
	End Sub

	' Token: 0x06002962 RID: 10594 RVA: 0x00181D7A File Offset: 0x0018017A
	Public Sub Extend()
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x06002963 RID: 10595 RVA: 0x00181D89 File Offset: 0x00180189
	Public Sub Retract()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.retract_cr())
	End Sub

	' Token: 0x06002964 RID: 10596 RVA: 0x00181DA0 File Offset: 0x001801A0
	Private Iterator Function main_cr() As IEnumerator
		Dim extendTime As Single = 4.45F
		Dim t As Single = 0F
		While t < extendTime
			MyBase.transform.SetPosition(New Single?(Me.parent.transform.position.x), New Single?(Me.parent.transform.position.y + Mathf.Lerp(-250F, 195F, t / extendTime)), Nothing)
			t += CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		MyBase.GetComponentInChildren(Of Collider2D)().enabled = True
		While True
			Dim rotation As Single = Me.tongueSpinner.eulerAngles.z
			Me.tongueSpinner.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(rotation + Me.properties.tongueRotateSpeed * CupheadTime.FixedDelta * -1F))
			MyBase.transform.SetPosition(New Single?(Me.parent.transform.position.x), New Single?(Me.parent.transform.position.y + 195F), Nothing)
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06002965 RID: 10597 RVA: 0x00181DBC File Offset: 0x001801BC
	Private Iterator Function retract_cr() As IEnumerator
		Dim retractTime As Single = 4.45F
		MyBase.GetComponentInChildren(Of Collider2D)().enabled = False
		Dim t As Single = 0F
		While t < retractTime
			MyBase.transform.SetPosition(New Single?(Me.parent.transform.position.x), New Single?(Me.parent.transform.position.y + Mathf.Lerp(195F, -250F, t / retractTime)), Nothing)
			t += CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x04003260 RID: 12896
	Private Const RETRACTED_Y_OFFSET As Single = -250F

	' Token: 0x04003261 RID: 12897
	Private Const EXTENDED_Y_OFFSET As Single = 195F

	' Token: 0x04003262 RID: 12898
	Private Const EXTEND_MOVE_SPEED As Single = 100F

	' Token: 0x04003263 RID: 12899
	Private properties As LevelProperties.RetroArcade.Worm

	' Token: 0x04003264 RID: 12900
	<SerializeField()>
	Private tongueSpinner As Transform

	' Token: 0x04003265 RID: 12901
	<SerializeField()>
	Private parent As RetroArcadeWorm

	' Token: 0x04003267 RID: 12903
	Private damageDealer As DamageDealer
End Class
