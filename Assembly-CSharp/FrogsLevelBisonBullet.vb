Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006A8 RID: 1704
Public Class FrogsLevelBisonBullet
	Inherits AbstractFrogsLevelSlotBullet

	' Token: 0x170003AB RID: 939
	' (get) Token: 0x0600241F RID: 9247 RVA: 0x001532B2 File Offset: 0x001516B2
	Protected Overrides ReadOnly Property Y_Ease As EaseUtils.EaseType
		Get
			Return EaseUtils.EaseType.easeOutElastic
		End Get
	End Property

	' Token: 0x170003AC RID: 940
	' (get) Token: 0x06002420 RID: 9248 RVA: 0x001532B6 File Offset: 0x001516B6
	Protected Overrides ReadOnly Property Y As Single
		Get
			Return -60F
		End Get
	End Property

	' Token: 0x170003AD RID: 941
	' (get) Token: 0x06002421 RID: 9249 RVA: 0x001532BD File Offset: 0x001516BD
	Protected Overrides ReadOnly Property Y_Time As Single
		Get
			Return 2F
		End Get
	End Property

	' Token: 0x06002422 RID: 9250 RVA: 0x001532C4 File Offset: 0x001516C4
	Public Function Create(pos As Vector2, s As Single, direction As FrogsLevelBisonBullet.Direction, bigX As Single, smallX As Single) As FrogsLevelBisonBullet
		Dim frogsLevelBisonBullet As FrogsLevelBisonBullet = TryCast(MyBase.Create(pos, s), FrogsLevelBisonBullet)
		frogsLevelBisonBullet.Init(direction, bigX, smallX)
		Return frogsLevelBisonBullet
	End Function

	' Token: 0x06002423 RID: 9251 RVA: 0x001532EC File Offset: 0x001516EC
	Private Sub Init(dir As FrogsLevelBisonBullet.Direction, big As Single, small As Single)
		Me.flame.GetComponent(Of Collider2D)().enabled = False
		AddHandler Me.flame.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf MyBase.DealDamage
		Me.direction = dir
		Me.bigX = big
		MyBase.StartCoroutine(Me.bison_cr())
		MyBase.StartCoroutine(Me.small_cr())
	End Sub

	' Token: 0x06002424 RID: 9252 RVA: 0x00153350 File Offset: 0x00151750
	Private Iterator Function small_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		Me.flame.GetComponent(Of Collider2D)().enabled = True
		MyBase.animator.SetTrigger("Small")
		Return
	End Function

	' Token: 0x06002425 RID: 9253 RVA: 0x0015336C File Offset: 0x0015176C
	Private Iterator Function bison_cr() As IEnumerator
		If Me.direction = FrogsLevelBisonBullet.Direction.Down Then
			Me.flame.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(180F))
			Me.flame.AddLocalPosition(0F, -115F, 0F)
			Me.flame.GetComponent(Of SpriteRenderer)().sortingOrder = MyBase.GetComponent(Of SpriteRenderer)().sortingOrder - 1
		End If
		Yield Nothing
		Yield Nothing
		Yield Nothing
		Dim big As Boolean = False
		While True
			Dim distance As Single = Single.MaxValue
			Dim p As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			Dim p2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			If p IsNot Nothing Then
				distance = Mathf.Min(distance, MyBase.transform.position.x - p.center.x)
			End If
			If p2 IsNot Nothing Then
				distance = Mathf.Min(distance, MyBase.transform.position.x - p2.center.x)
			End If
			If distance <= Me.bigX AndAlso Not big Then
				big = True
				AudioManager.Play("level_frogs_flame_platform_fire_burst")
				Me.emitAudioFromObject.Add("level_frogs_flame_platform_fire_burst")
				AudioManager.PlayLoop("level_frogs_flame_platform_fire_loop")
				Me.emitAudioFromObject.Add("level_frogs_flame_platform_fire_loop")
				Me.flame.GetComponent(Of Collider2D)().enabled = True
				MyBase.animator.SetTrigger("Big")
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002426 RID: 9254 RVA: 0x00153387 File Offset: 0x00151787
	Protected Overrides Sub [End]()
		AudioManager.[Stop]("level_frogs_flame_platform_fire_loop")
		MyBase.[End]()
	End Sub

	' Token: 0x04002CE5 RID: 11493
	Public flame As Transform

	' Token: 0x04002CE6 RID: 11494
	Private direction As FrogsLevelBisonBullet.Direction

	' Token: 0x04002CE7 RID: 11495
	Private bigX As Single

	' Token: 0x020006A9 RID: 1705
	Public Enum Direction
		' Token: 0x04002CE9 RID: 11497
		Up
		' Token: 0x04002CEA RID: 11498
		Down
	End Enum
End Class
