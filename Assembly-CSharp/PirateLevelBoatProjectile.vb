Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000720 RID: 1824
Public Class PirateLevelBoatProjectile
	Inherits AbstractProjectile

	' Token: 0x060027B2 RID: 10162 RVA: 0x001740D8 File Offset: 0x001724D8
	Public Function Create(pos As Vector2, speed As Single, rotationSpeed As Single) As PirateLevelBoatProjectile
		Dim pirateLevelBoatProjectile As PirateLevelBoatProjectile = TryCast(Me.Create(), PirateLevelBoatProjectile)
		pirateLevelBoatProjectile.CollisionDeath.OnlyPlayer()
		pirateLevelBoatProjectile.DamagesType.OnlyPlayer()
		pirateLevelBoatProjectile.Init(pos, speed, rotationSpeed)
		Return pirateLevelBoatProjectile
	End Function

	' Token: 0x060027B3 RID: 10163 RVA: 0x00174112 File Offset: 0x00172512
	Private Sub Init(pos As Vector2, speed As Single, rotationSpeed As Single)
		MyBase.StartCoroutine(Me.bullet_cr(pos, speed, rotationSpeed))
	End Sub

	' Token: 0x060027B4 RID: 10164 RVA: 0x00174124 File Offset: 0x00172524
	Protected Overrides Sub Update()
		MyBase.Update()
		Me.child.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
	End Sub

	' Token: 0x060027B5 RID: 10165 RVA: 0x00174155 File Offset: 0x00172555
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
			Me.Die()
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.die_cr())
		End If
	End Sub

	' Token: 0x060027B6 RID: 10166 RVA: 0x00174184 File Offset: 0x00172584
	Protected Overrides Sub Die()
		Me.child.SetLocalEulerAngles(New Single?(0F), New Single?(0F), New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
		MyBase.Die()
	End Sub

	' Token: 0x060027B7 RID: 10167 RVA: 0x001741BC File Offset: 0x001725BC
	Private Sub [End]()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060027B8 RID: 10168 RVA: 0x001741D0 File Offset: 0x001725D0
	Private Iterator Function bullet_cr(pos As Vector2, speed As Single, rotationSpeed As Single) As IEnumerator
		MyBase.transform.position = pos - Me.child.localPosition
		TryCast(MyBase.GetComponent(Of Collider2D)(), CircleCollider2D).offset = Me.child.localPosition
		While True
			If MyBase.transform.position.x < -1280F Then
				Me.[End]()
			End If
			MyBase.transform.AddPosition(-speed * CupheadTime.Delta, 0F, 0F)
			MyBase.transform.AddEulerAngles(0F, 0F, -rotationSpeed * CupheadTime.Delta)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060027B9 RID: 10169 RVA: 0x00174200 File Offset: 0x00172600
	Private Iterator Function die_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04003074 RID: 12404
	<SerializeField()>
	Private child As Transform
End Class
