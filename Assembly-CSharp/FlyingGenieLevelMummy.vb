Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000671 RID: 1649
Public Class FlyingGenieLevelMummy
	Inherits BasicProjectile

	' Token: 0x17000397 RID: 919
	' (get) Token: 0x060022B0 RID: 8880 RVA: 0x00145C68 File Offset: 0x00144068
	' (set) Token: 0x060022B1 RID: 8881 RVA: 0x00145C70 File Offset: 0x00144070
	Public Property type As FlyingGenieLevelMummy.MummyType

	' Token: 0x060022B2 RID: 8882 RVA: 0x00145C7C File Offset: 0x0014407C
	Public Function Create(position As Vector3, speed As Single, rotation As Single, properties As LevelProperties.FlyingGenie.Coffin, type As FlyingGenieLevelMummy.MummyType, hp As Single, sortingOrder As Integer) As FlyingGenieLevelMummy
		Dim flyingGenieLevelMummy As FlyingGenieLevelMummy = TryCast(MyBase.Create(position, rotation, speed), FlyingGenieLevelMummy)
		flyingGenieLevelMummy.transform.position = position
		flyingGenieLevelMummy.properties = properties
		flyingGenieLevelMummy.type = type
		flyingGenieLevelMummy.hp = hp
		flyingGenieLevelMummy.rotation = rotation
		flyingGenieLevelMummy.GetComponent(Of SpriteRenderer)().sortingOrder = sortingOrder
		flyingGenieLevelMummy.purpleSprite.sortingOrder = sortingOrder + 1
		flyingGenieLevelMummy.purpleColor = Me.purpleSprite.color
		Return flyingGenieLevelMummy
	End Function

	' Token: 0x060022B3 RID: 8883 RVA: 0x00145CF6 File Offset: 0x001440F6
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060022B4 RID: 8884 RVA: 0x00145D24 File Offset: 0x00144124
	Protected Overrides Sub Start()
		MyBase.Start()
		AudioManager.Play("genie_mummy_voice_attack")
		Me.emitAudioFromObject.Add("genie_mummy_voice_attack")
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.fade_purple_cr())
		Dim type As FlyingGenieLevelMummy.MummyType = Me.type
		If type <> FlyingGenieLevelMummy.MummyType.Classic Then
			If type <> FlyingGenieLevelMummy.MummyType.Chomper Then
				If type = FlyingGenieLevelMummy.MummyType.Grabby Then
					MyBase.animator.Play("Grabby")
				End If
			Else
				MyBase.animator.Play("Chomper")
			End If
		Else
			Me.CalculateSin()
			If Me.properties.mummyASinWave Then
				MyBase.StartCoroutine(Me.classic_bounce_cr())
			End If
			MyBase.animator.Play("Classic")
		End If
	End Sub

	' Token: 0x060022B5 RID: 8885 RVA: 0x00145E04 File Offset: 0x00144204
	Private Iterator Function fade_purple_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 1.5F
		Dim start As Color = Me.purpleSprite.color
		Dim [end] As Color = New Color(1F, 1F, 1F, 0F)
		While t < time
			Me.purpleSprite.color = Color.Lerp(start, [end], t / time)
			Me.purpleColor = Me.purpleSprite.color
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060022B6 RID: 8886 RVA: 0x00145E20 File Offset: 0x00144220
	Private Sub CalculateSin()
		Dim vector As Vector3 = MathUtils.AngleToDirection(Me.rotation)
		Dim zero As Vector2 = Vector2.zero
		zero.x = (vector.x + MyBase.transform.position.x) / 2F
		zero.y = (vector.y + MyBase.transform.position.y) / 2F
		Dim num As Single = -((vector.x - MyBase.transform.position.x) / (vector.y - MyBase.transform.position.y))
		Dim num2 As Single = zero.y - num * zero.x
		Dim zero2 As Vector2 = Vector2.zero
		zero2.x = zero.x + 1F
		zero2.y = num * zero2.x + num2
		Me.normalized = Vector3.zero
		Me.normalized = zero2 - zero
		Me.normalized.Normalize()
	End Sub

	' Token: 0x060022B7 RID: 8887 RVA: 0x00145F3C File Offset: 0x0014433C
	Private Iterator Function classic_bounce_cr() As IEnumerator
		Dim pos As Vector3 = MyBase.transform.position
		Dim angle As Single = 0F
		While True
			angle += 10F * CupheadTime.Delta
			If CupheadTime.Delta IsNot 0F Then
				pos = Me.normalized * Mathf.Sin(angle) * 2F
				MyBase.transform.position += pos
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060022B8 RID: 8888 RVA: 0x00145F58 File Offset: 0x00144358
	Private Iterator Function grabby_speed_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.5F
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			Me.Speed = Mathf.Lerp(-Me.properties.mummyCSpeed, 0F, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060022B9 RID: 8889 RVA: 0x00145F73 File Offset: 0x00144373
	Private Sub ChangeSpeed()
		If Me.properties.mummyCSlowdown Then
			Me.Speed = -Me.properties.mummyCSpeed
			MyBase.StartCoroutine(Me.grabby_speed_cr())
		End If
	End Sub

	' Token: 0x060022BA RID: 8890 RVA: 0x00145FA4 File Offset: 0x001443A4
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		Me.Explosion()
		MyBase.Die()
		AudioManager.[Stop]("genie_mummy_voice_attack")
		AudioManager.Play("genie_mummy_voice_die")
		Me.emitAudioFromObject.Add("genie_mummy_voice_die")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
	End Sub

	' Token: 0x060022BB RID: 8891 RVA: 0x00145FFF File Offset: 0x001443FF
	Private Sub Explosion()
		Me.sparkFX.Create(MyBase.transform.position, Me.purpleColor)
	End Sub

	' Token: 0x04002B4E RID: 11086
	<SerializeField()>
	Private sparkFX As FlyingGenieLevelMummyDeathEffect

	' Token: 0x04002B4F RID: 11087
	<SerializeField()>
	Private purpleSprite As SpriteRenderer

	' Token: 0x04002B51 RID: 11089
	Private properties As LevelProperties.FlyingGenie.Coffin

	' Token: 0x04002B52 RID: 11090
	Private normalized As Vector3

	' Token: 0x04002B53 RID: 11091
	Private damageReceiver As DamageReceiver

	' Token: 0x04002B54 RID: 11092
	Private hp As Single

	' Token: 0x04002B55 RID: 11093
	Private rotation As Single

	' Token: 0x04002B56 RID: 11094
	Private purpleColor As Color

	' Token: 0x02000672 RID: 1650
	Public Enum MummyType
		' Token: 0x04002B58 RID: 11096
		Classic
		' Token: 0x04002B59 RID: 11097
		Chomper
		' Token: 0x04002B5A RID: 11098
		Grabby
	End Enum
End Class
