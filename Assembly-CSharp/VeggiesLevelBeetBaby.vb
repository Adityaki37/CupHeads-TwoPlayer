Imports System
Imports UnityEngine

' Token: 0x0200083D RID: 2109
Public Class VeggiesLevelBeetBaby
	Inherits AbstractCollidableObject

	' Token: 0x060030E0 RID: 12512 RVA: 0x001CBCB4 File Offset: 0x001CA0B4
	Public Function Create(type As VeggiesLevelBeetBaby.Type, speed As Single, childSpeed As Single, range As Single, pos As Vector2, rot As Single) As VeggiesLevelBeetBaby
		Dim veggiesLevelBeetBaby As VeggiesLevelBeetBaby = Me.InstantiatePrefab(Of VeggiesLevelBeetBaby)()
		veggiesLevelBeetBaby.Init(type, speed, childSpeed, range, pos, rot)
		Return veggiesLevelBeetBaby
	End Function

	' Token: 0x060030E1 RID: 12513 RVA: 0x001CBCD8 File Offset: 0x001CA0D8
	Private Sub Init(type As VeggiesLevelBeetBaby.Type, speed As Single, childSpeed As Single, range As Single, pos As Vector2, rot As Single)
		Me.type = type
		Me.speed = speed
		Me.childSpeed = childSpeed
		Me.range = range
		MyBase.transform.position = pos
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(rot))
		MyBase.animator.Play(type.ToString())
		Me.damageDealer = New DamageDealer(1F, 0.2F, True, False, False)
		Me.damageDealer.SetDirection(DamageDealer.Direction.Neutral, MyBase.transform)
	End Sub

	' Token: 0x060030E2 RID: 12514 RVA: 0x001CBD7C File Offset: 0x001CA17C
	Private Sub Update()
		If Me.state = VeggiesLevelBeetBaby.State.Dead Then
			Return
		End If
		MyBase.transform.position += MyBase.transform.right * Me.speed * CupheadTime.Delta
		If MyBase.transform.position.y > 360F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060030E3 RID: 12515 RVA: 0x001CBDF4 File Offset: 0x001CA1F4
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060030E4 RID: 12516 RVA: 0x001CBE0C File Offset: 0x001CA20C
	Private Sub Die()
		Me.state = VeggiesLevelBeetBaby.State.Dead
		MyBase.animator.SetTrigger("Explode")
		Dim num As Integer = If((Me.type <> VeggiesLevelBeetBaby.Type.Fat), 3, 5)
		For i As Integer = 0 To num - 1
			Dim num2 As Single = CSng(i) / CSng((num - 1))
			Dim num3 As Single = Mathf.Lerp(0F, Me.range, num2) - 90F - Me.range / 2F
			Dim veggiesLevelBeetBabyBullet As VeggiesLevelBeetBabyBullet = Me.bulletPrefab.Create(Me.childSpeed, MyBase.transform.position, num3)
			If Me.type = VeggiesLevelBeetBaby.Type.Pink Then
				veggiesLevelBeetBabyBullet.SetParryable(True)
			End If
		Next
	End Sub

	' Token: 0x060030E5 RID: 12517 RVA: 0x001CBEBB File Offset: 0x001CA2BB
	Private Sub OnDeathAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04003981 RID: 14721
	Private Const BULLET_COUNT As Integer = 3

	' Token: 0x04003982 RID: 14722
	Private Const BULLET_COUNT_FAT As Integer = 5

	' Token: 0x04003983 RID: 14723
	<SerializeField()>
	Private bulletPrefab As VeggiesLevelBeetBabyBullet

	' Token: 0x04003984 RID: 14724
	Private type As VeggiesLevelBeetBaby.Type

	' Token: 0x04003985 RID: 14725
	Private speed As Single

	' Token: 0x04003986 RID: 14726
	Private childSpeed As Single

	' Token: 0x04003987 RID: 14727
	Private range As Single

	' Token: 0x04003988 RID: 14728
	Private state As VeggiesLevelBeetBaby.State

	' Token: 0x04003989 RID: 14729
	Private damageDealer As DamageDealer

	' Token: 0x0200083E RID: 2110
	Public Enum Type
		' Token: 0x0400398B RID: 14731
		Regular
		' Token: 0x0400398C RID: 14732
		Fat
		' Token: 0x0400398D RID: 14733
		Pink
	End Enum

	' Token: 0x0200083F RID: 2111
	Public Enum State
		' Token: 0x0400398F RID: 14735
		Go
		' Token: 0x04003990 RID: 14736
		Dead
	End Enum
End Class
