CREATE DATABASE GMR;

USE GMR;

CREATE TABLE usuarios
(
	usuario_id INT PRIMARY KEY IDENTITY,
	correo VARCHAR(40),
	contrasena VARCHAR(40),
	nombre VARCHAR(40),
    perfilURL VARCHAR(255) NULL,
	fechaInscripcion DATE,
	rol TINYINT DEFAULT 1,
	baja TINYINT DEFAULT 0
);

INSERT INTO Usuarios(correo, contrasena, nombre, fechaInscripcion, rol) 
			VALUES ('admingod@gmail.com','fulladmin','god','2025-09-25',0)

CREATE TABLE juegos(
	juego_id INT IDENTITY PRIMARY KEY,
	nombre VARCHAR(30),
	descripcion NVARCHAR(1000),
	imagenURL NVARCHAR(MAX),
	fecha_publicacion DATE,
	desarrollador VARCHAR(50),
	editor VARCHAR(30),
	plataforma VARCHAR(30),
	baja TINYINT
);

-----------PARA EL CAROUSEL-----------
CREATE TABLE proxJuegos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(200) NOT NULL,
    FotoUrl NVARCHAR(500) NOT NULL
);

CREATE TABLE tags(
	tag_id INT IDENTITY PRIMARY KEY,
	nombre VARCHAR(30),
	baja TINYINT
);

CREATE TABLE juegoXtag(
	juegoXtag_id INT IDENTITY PRIMARY KEY,
	juego_id INT,
	tag_id INT,
	baja TINYINT DEFAULT 0,
	FOREIGN KEY (juego_id) REFERENCES juegos(juego_id),
	FOREIGN KEY (tag_id) REFERENCES tags(tag_id)
);

CREATE TABLE comentarios(
	comentario_id INT IDENTITY PRIMARY KEY,
	comentario VARCHAR(1000),
	puntuacion TINYINT,
	usuario_id INT,
	juego_id INT,
	baja TINYINT,
	FOREIGN KEY (usuario_id) REFERENCES usuarios(usuario_id),
	FOREIGN KEY (juego_id) REFERENCES juegos(juego_id)
);

CREATE TABLE respuestas(
	repuesta_id INT IDENTITY PRIMARY KEY,
	comentario VARCHAR(1000),
	comentario_id INT,
	usuario_id INT,
	baja TINYINT,
	FOREIGN KEY (comentario_id) REFERENCES comentarios(comentario_id),
	FOREIGN KEY (usuario_id) REFERENCES usuarios(usuario_id)
);

CREATE TABLE juegosFavoritos(
	juegoFavorito INT IDENTITY PRIMARY KEY,
	usuario_id INT,
	juego_id INT,
	baja TINYINT,
	FOREIGN KEY (juego_id) REFERENCES juegos(juego_id),
	FOREIGN KEY (usuario_id) REFERENCES usuarios(usuario_id)
);

CREATE TABLE comentario_likes (
    like_id INT IDENTITY PRIMARY KEY,
    comentario_id INT NOT NULL,
    usuario_id INT NOT NULL,
    baja TINYINT DEFAULT 0, -- 0 = like activo, 1 = eliminado
    fecha DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Like_Comentario FOREIGN KEY (comentario_id) REFERENCES comentarios(comentario_id),
    CONSTRAINT FK_Like_Usuario FOREIGN KEY (usuario_id) REFERENCES usuarios(usuario_id),
    CONSTRAINT UQ_ComentarioUsuario UNIQUE (comentario_id, usuario_id) -- un like por usuario por comentario
);


/*------------------------------------------------STORED PROCEDURES-----------------------------------------------------*/

/*----------------LOGIN----------------*/

/*----------------sp insercion login usuario----------------*/

CREATE PROCEDURE sp_AddNewUserLogin
	(@correo VARCHAR(30),
	@contrasena VARCHAR(30),
	@nombre VARCHAR(30),
	@fechaInscripcion DATE
	)
AS
BEGIN
	
	IF EXISTS (SELECT 1 /*revisa que no existe el user*/
				FROM Usuarios
					WHERE correo = @correo)
	BEGIN
		RETURN 1
	END
	ELSE
	BEGIN
		INSERT INTO Usuarios(correo, contrasena, nombre, fechaInscripcion) 
			VALUES (@correo,@contrasena,@nombre,@fechaInscripcion)

		RETURN 0
	END
END;

/*----------------sp update all----------------*/

CREATE PROCEDURE sp_UpdateUser
    @usuario_id INT,
    @correo VARCHAR(40) = NULL,
    @contrasena VARCHAR(40) = NULL,
    @nombre VARCHAR(40) = NULL,
    @perfilURL VARCHAR(200) = NULL
AS
BEGIN
    IF NOT EXISTS(SELECT 1 FROM Usuarios WHERE usuario_id = @usuario_id)
        RETURN 1; -- Usuario no encontrado

    UPDATE Usuarios
    SET
        correo = ISNULL(@correo, correo),
        contrasena = ISNULL(@contrasena, contrasena),
        nombre = ISNULL(@nombre, nombre),
        perfilURL = ISNULL(@perfilURL, perfilURL)
    WHERE usuario_id = @usuario_id;

    RETURN 0; -- Ok
END

/*----------------sp obtencion de usuarios con id (para el UserScreen)----------------*/

CREATE PROCEDURE sp_GetUserById
(
    @usuario_id INT
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE usuario_id = @usuario_id)
        RETURN 1; -- Usuario no encontrado
    ELSE
    BEGIN
        SELECT usuario_id, correo, nombre, rol, perfilURL, baja
        FROM Usuarios
        WHERE usuario_id = @usuario_id;

        RETURN 0; -- OK
    END
END;

/*----------------sp obtencion icono por usuario con id----------------*/

CREATE PROCEDURE sp_GetIconUSer
(
	@usuario_id INT
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM usuarios WHERE usuario_id = @usuario_id)
		RETURN 1
	ELSE 
	BEGIN
		SELECT perfilURL FROM usuarios WHERE usuario_id = @usuario_id;
		RETURN 0
	END
END;

/*----------------GAMES----------------*/

/*----------------sp insercion game----------------*/

CREATE PROCEDURE sp_AddNewGame
(
    @nombre VARCHAR(30),
    @descripcion NVARCHAR(MAX),
    @fechaCreacion DATE,
    @desarrollador VARCHAR(30),
    @editor VARCHAR(30),
    @plataforma VARCHAR(30),
    @imagen NVARCHAR(255) -- Ruta/nombre de archivo
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM juegos WHERE nombre = @nombre)
        RETURN 1
    ELSE
    BEGIN
        INSERT INTO juegos
        (
            nombre,
            descripcion,
            fecha_publicacion,
            desarrollador,
            editor,
            plataforma,
            imagenURL,
            baja
        )
        VALUES
        (
            @nombre,
            @descripcion,
            @fechaCreacion,
            @desarrollador,
            @editor,
            @plataforma,
            @imagen,
            0
        );

        RETURN 0;
    END
END;

/*----------------sp obtener todos los games----------------*/

CREATE PROCEDURE sp_GetGames
AS
BEGIN
    SELECT 
        juego_id AS Id,
        nombre,
        descripcion,
        fecha_publicacion AS FechaPublicacion,
        desarrollador,
		editor,
		plataforma,
        imagenURL
    FROM juegos
    WHERE baja = 0
END;

/*----------------sp obtener un juego por id----------------*/

CREATE PROCEDURE sp_GetOneGame
(
	@game_id INT
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM juegos WHERE juego_id = @game_id)
		RETURN 1
	ELSE
	BEGIN
		SELECT juegos.nombre, juegos.descripcion, juegos.fecha_publicacion AS FechaPublicacion, juegos.desarrollador, juegos.editor, juegos.plataforma, juegos.imagenURL FROM juegos WHERE juego_id = @game_id AND baja = 0
	END
END;

/*----------------sp obtener todos los juegos con paginacion para lazy loading----------------*/

CREATE PROCEDURE sp_GetGamesLazy
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize < 1 SET @PageSize = 20;
    IF @PageSize > 100 SET @PageSize = 100; -- Max limit to prevent performance issues
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*) 
    FROM juegos 
    WHERE baja = 0;
    
    SELECT 
        juego_id AS Id,
        nombre,
        descripcion,
        fecha_publicacion AS FechaPublicacion,
        desarrollador,
        editor,
        plataforma,
        imagenURL,
        @TotalRecords AS TotalRecords,
        @PageNumber AS CurrentPage,
        @PageSize AS PageSize,
        CEILING(CAST(@TotalRecords AS FLOAT) / @PageSize) AS TotalPages
    FROM juegos
    WHERE baja = 0
    ORDER BY juego_id DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;

/*----------------sp obtener mis juegos por id----------------*/

CREATE PROCEDURE sp_GetMyGames
    @usuario_id INT
AS
BEGIN
    SELECT 
        j.juego_id AS Id,
        j.nombre,
        j.descripcion,
        j.imagenURL,
        j.fecha_publicacion,
        j.desarrollador,
		j.editor,
		j.plataforma
    FROM juegos j
    INNER JOIN juegosFavoritos f ON j.juego_id = f.juego_id
    WHERE f.usuario_id = @usuario_id
      AND f.baja = 0
      AND (j.baja = 0 OR j.baja IS NULL); -- opcional: solo juegos activos
END;

/*----------------sp editar un juego por id----------------*/

CREATE PROCEDURE sp_EditeGameById
(
	@juego_id INT,
	@nombre VARCHAR(30),
	@descripcion NVARCHAR(MAX),
	@fechaCreacion DATE,
	@desarrollador VARCHAR(30),
	@editor VARCHAR(30),
	@plataforma VARCHAR(30),
	@imagenURL NVARCHAR(MAX) = NULL -- 👈 NUEVO
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM juegos WHERE juego_id = @juego_id AND baja = 0)
		RETURN 1
	ELSE
	BEGIN
		UPDATE juegos SET
			nombre = @nombre, 
			descripcion = @descripcion, 
			fecha_publicacion = @fechaCreacion, 
			desarrollador = @desarrollador, 
			editor = @editor ,
			plataforma = @plataforma,
			imagenURL = ISNULL(@imagenURL, imagenURL) -- 👈 actualiza solo si viene
		WHERE juego_id = @juego_id 
		RETURN 0
	END
END;

/*----------------sp eliminar un juego por id----------------*/

CREATE PROCEDURE SP_Update_Juego_Baja
    @JuegoId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE juegos
    SET baja = 1
    WHERE juego_id = @JuegoId;

    IF @@ROWCOUNT > 0
        SELECT 1 AS Success, 'Juego dado de baja correctamente' AS Message;
    ELSE
        SELECT 0 AS Success, 'No se encontró el juego o ya estaba dado de baja' AS Message;
END


/*----------------TAGS----------------*/

/*----------------sp insercion tag----------------*/

CREATE PROCEDURE sp_AddNewTags
	(
		@nombre VARCHAR (40)
	)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM tags WHERE nombre = @nombre)
		RETURN 1
	ELSE
	BEGIN
		INSERT INTO tags(nombre,baja) VALUES (@nombre,0)
		RETURN 0
	END
END;

/*----------------sp obtener todos los tags----------------*/

CREATE PROCEDURE sp_GetAllTags
AS
BEGIN
	SELECT tag_id, nombre FROM tags WHERE baja = 0
END;

/*----------------sp eliminar tag por id----------------*/

CREATE PROCEDURE sp_DeleteTag
(
	@tag_id INT
)
AS
BEGIN
	UPDATE tags SET baja = 1 WHERE tag_id = @tag_id;
END;

/*----------------sp insercion tagXjuego----------------*/

CREATE PROCEDURE sp_InsertTagXGame
	(
		@juego_id INT,
		@tag_id INT
	)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM juegoXtag WHERE juego_id = @juego_id AND tag_id = @tag_id AND baja = 0)
		RETURN 1
	ELSE
	BEGIN
		IF EXISTS (SELECT 1 FROM juegoXtag WHERE juego_id = @juego_id AND tag_id = @tag_id AND baja = 1)
		BEGIN
			UPDATE juegoXtag SET baja = 0 WHERE juego_id = @juego_id AND tag_id = @tag_id;
			RETURN 2
		END
	ELSE
		INSERT INTO juegoXtag(juego_id,tag_id) VALUES (@juego_id,@tag_id)
		RETURN 0
	END
END;

/*----------------sp obtencion tagXjuego----------------*/

CREATE PROCEDURE sp_GetTagXGame
(
	@juego_id INT
)
AS
BEGIN
	SELECT t.tag_id, t.nombre FROM tags AS t
	JOIN juegoXtag jXt ON t.tag_id = jXt.tag_id
	JOIN juegos j ON jXt.juego_id = j.juego_id
	WHERE j.juego_id = @juego_id AND jXt.baja = 0
END;

/*----------------sp eliminacion tagXjuego----------------*/

CREATE PROCEDURE sp_DeleteTagXGame
(
	@juego_id INT,
	@tag_id INT
)
AS
BEGIN
	UPDATE juegoXtag SET baja = 1 WHERE juego_id = @juego_id AND tag_id = @tag_id;
END;

/*----------------JUEGOS FAVORITOS----------------*/

/*----------------sp Insercion y Modificacion juego favorito----------------*/

CREATE PROCEDURE sp_AddOrRemoveFavorite
(
	@usuario_id INT,
	@juego_id INT,
	@botonCheck BIT /*obtengo el valor del check (1 o 2)*/
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @botonCheck = 1
    BEGIN
        -- Si ya existe una fila, reactiva (baja = 0), sino inserta
        IF EXISTS(SELECT 1 FROM juegosFavoritos WHERE usuario_id = @usuario_id AND juego_id = @juego_id)
        BEGIN
            UPDATE juegosFavoritos
            SET baja = 0
            WHERE usuario_id = @usuario_id AND juego_id = @juego_id;
        END
        ELSE
        BEGIN
            INSERT INTO juegosFavoritos (usuario_id, juego_id, baja)
            VALUES (@usuario_id, @juego_id, 0);
        END
    END
    ELSE
    BEGIN
        -- Soft delete (marca baja = 1)
        UPDATE juegosFavoritos
        SET baja = 1
        WHERE usuario_id = @usuario_id AND juego_id = @juego_id;
    END
END;

/*----------------sp para obtener los favoritos----------------*/
CREATE PROCEDURE sp_IsFavoriteGame
    @usuario_id INT,
    @juego_id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM juegosfavoritos
        WHERE usuario_id = @usuario_id 
          AND juego_id = @juego_id 
          AND baja = 0
    )
    BEGIN
        SELECT CAST(1 AS BIT) AS isFavorite;
    END
    ELSE
    BEGIN
        SELECT CAST(0 AS BIT) AS isFavorite;
    END
END;


/*----------------COMENTARIOS----------------*/

/*----------------sp obtener todos los comentario----------------*/
CREATE PROCEDURE sp_GetAllComentarios
    @juego_id INT
AS
BEGIN
    SELECT 
        c.comentario_id,
        c.comentario,
        c.puntuacion,
        c.usuario_id,
        u.nombre AS usuarioNombre, -- 👈 nombre del usuario
        c.juego_id,
        c.baja
    FROM comentarios c
    INNER JOIN Usuarios u ON c.usuario_id = u.usuario_id
    WHERE c.juego_id = @juego_id
      AND c.baja = 0;
END;


/*----------------sp Insercion comentario----------------*/

CREATE PROCEDURE sp_AddComentario
--Inserta un comentario en la tabla, validando que existan el usuario_id y el juego_id.
    @comentario NVARCHAR(1000),
    @puntuacion TINYINT,
    @usuario_id INT,
    @juego_id INT
AS
BEGIN
    -- Validar existencia de usuario
    IF NOT EXISTS (SELECT 1 FROM usuarios WHERE usuario_id = @usuario_id)
    BEGIN
        RETURN 1; -- Usuario no existe
    END

    -- Validar existencia de juego
    IF NOT EXISTS (SELECT 1 FROM juegos WHERE juego_id = @juego_id)
    BEGIN
        RETURN 2; -- Juego no existe
    END

    -- Insertar comentario
    INSERT INTO comentarios (comentario, puntuacion, usuario_id, juego_id, baja)
    VALUES (@comentario, @puntuacion, @usuario_id, @juego_id, 0);

    RETURN 0; -- OK
END;

/*----------------sp obtencion comentario----------------*/

--Devuelve todos los comentarios de un juego, con datos del usuario.

CREATE PROCEDURE sp_GetComentariosByJuego
    @juego_id INT
AS
BEGIN
    SELECT 
        c.comentario_id,
        c.comentario,
        c.puntuacion,
        c.usuario_id,
        u.nombre AS UsuarioNombre,
		u.perfilURL,
        c.juego_id,
        c.baja
    FROM comentarios c
    INNER JOIN usuarios u ON u.usuario_id = c.usuario_id
    WHERE c.juego_id = @juego_id AND c.baja = 0;
END;

/*----------------sp eliminacion comentario----------------*/

CREATE PROCEDURE sp_DeleteComentario
    @comentario_id INT
AS
BEGIN
    -- Validar si el comentario existe y no está dado de baja
    IF NOT EXISTS (SELECT 1 FROM comentarios WHERE comentario_id = @comentario_id AND baja = 0)
    BEGIN
        RETURN 1; -- No existe o ya está dado de baja
    END

    -- Dar de baja primero las respuestas asociadas
    UPDATE respuestas
    SET baja = 1
    WHERE comentario_id = @comentario_id;

    -- Dar de baja el comentario
    UPDATE comentarios
    SET baja = 1
    WHERE comentario_id = @comentario_id;

    RETURN 0; -- OK
END;

/*----------------sp modificacion comentario----------------*/

CREATE PROCEDURE sp_UpdateComentario
    @comentario_id INT,
    @nuevoTexto NVARCHAR(1000),
    @nuevaPuntuacion TINYINT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM comentarios WHERE comentario_id = @comentario_id AND baja = 0)
    BEGIN
        RETURN 1; -- No existe
    END

    UPDATE comentarios
    SET comentario = @nuevoTexto,
        puntuacion = @nuevaPuntuacion
    WHERE comentario_id = @comentario_id;

    RETURN 0; -- OK
END;

/*----------------RESPUESTAS----------------*/

/*----------------sp insercion respuesta----------------*/

CREATE PROCEDURE sp_AddRespuesta
    @comentario NVARCHAR(1000),
    @comentario_id INT,
    @usuario_id INT
AS
BEGIN
    -- Validar existencia del comentario
    IF NOT EXISTS (SELECT 1 FROM comentarios WHERE comentario_id = @comentario_id)
    BEGIN
        RETURN 1; -- Comentario no existe
    END

    -- Validar existencia del usuario
    IF NOT EXISTS (SELECT 1 FROM usuarios WHERE usuario_id = @usuario_id)
    BEGIN
        RETURN 2; -- Usuario no existe
    END

    -- Insertar respuesta
    INSERT INTO respuestas (comentario, comentario_id, usuario_id, baja)
    VALUES (@comentario, @comentario_id, @usuario_id, 0);

    RETURN 0; -- OK
END;

/*----------------sp obtencion respuesta----------------*/

CREATE PROCEDURE sp_GetRespuestasByComentario
    @comentario_id INT
AS
BEGIN
    SELECT 
        r.respuesta_id,
        r.comentario AS respuestaTexto,
        r.comentario_id,
        r.usuario_id,
		u.perfilURL,
        u.nombre AS UsuarioNombre,
        r.baja
    FROM respuestas r
    INNER JOIN usuarios u ON u.usuario_id = r.usuario_id
    WHERE r.comentario_id = @comentario_id AND r.baja = 0;
END;

/*----------------sp eliminacion respuesta----------------*/

CREATE PROCEDURE sp_DeleteRespuesta
    @repuesta_id INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM respuestas WHERE repuesta_id = @repuesta_id)
    BEGIN
        RETURN 1; -- No existe
    END

    UPDATE respuestas
    SET baja = 1
    WHERE repuesta_id = @repuesta_id;

    RETURN 0; -- OK
END;

/*----------------sp modificacion respuesta----------------*/

CREATE PROCEDURE sp_UpdateRespuesta
    @repuesta_id INT,
    @nuevoTexto NVARCHAR(1000)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM respuestas WHERE repuesta_id = @repuesta_id AND baja = 0)
    BEGIN
        RETURN 1; -- No existe
    END

    UPDATE respuestas
    SET comentario = @nuevoTexto
    WHERE repuesta_id = @repuesta_id;

    RETURN 0; -- OK
END;

/*----------------CARROUSEL----------------*/

/*----------------sp agregar foto en carrousel----------------*/

CREATE PROCEDURE sp_AddNewProxJuego
    @nombre NVARCHAR(200),
    @imagen NVARCHAR(500)
AS
BEGIN
    INSERT INTO proxJuegos (Nombre, FotoUrl)
    VALUES (@nombre, @imagen);

    SELECT SCOPE_IDENTITY() AS NewId;
END;

/*----------------sp eliminar foto en carrousel----------------*/

CREATE PROCEDURE SP_Delete_ProxJuego
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM proxJuegos WHERE Id = @Id;

        SELECT 
            1 AS success,
            0 AS error,
            200 AS code,
            'Próximo juego eliminado correctamente' AS message;
    END TRY
    BEGIN CATCH
        SELECT 
            0 AS success,
            1 AS error,
            500 AS code,
            ERROR_MESSAGE() AS message;
    END CATCH
END;

/*------------- LIKE ------------------*/

/*----------------sp agregar like/dislike en comentario----------------*/

CREATE PROCEDURE sp_ToggleComentarioLike
    @comentario_id INT,
    @usuario_id INT
AS
BEGIN
    -- validar existencia comentario
    IF NOT EXISTS (SELECT 1 FROM comentarios WHERE comentario_id = @comentario_id AND baja = 0)
        RETURN 1; -- comentario no existe o dado de baja

    -- validar existencia usuario
    IF NOT EXISTS (SELECT 1 FROM usuarios WHERE usuario_id = @usuario_id)
        RETURN 2; -- usuario no existe

    -- si no existe registro, crear like nuevo
    IF NOT EXISTS (SELECT 1 FROM comentario_likes WHERE comentario_id = @comentario_id AND usuario_id = @usuario_id)
    BEGIN
        INSERT INTO comentario_likes (comentario_id, usuario_id, baja)
        VALUES (@comentario_id, @usuario_id, 0);
    END
    ELSE
    BEGIN
        -- si existe, toggle (0 -> 1 o 1 -> 0)
        UPDATE comentario_likes
        SET baja = CASE WHEN baja = 0 THEN 1 ELSE 0 END,
            fecha = GETDATE()
        WHERE comentario_id = @comentario_id AND usuario_id = @usuario_id;
    END

    -- devolver conteo actualizado
    SELECT 
        COUNT(*) AS LikeCount,
        MAX(CASE WHEN baja = 0 AND usuario_id = @usuario_id THEN 1 ELSE 0 END) AS Liked
    FROM comentario_likes
    WHERE comentario_id = @comentario_id AND baja = 0;

    RETURN 0;
END;


/*----------------sp obtener likes por comentario----------------*/

CREATE PROCEDURE sp_GetComentarioLike
    @comentario_id INT,
    @usuario_id INT
AS
BEGIN
    -- validar existencia comentario
    IF NOT EXISTS (SELECT 1 FROM comentarios WHERE comentario_id = @comentario_id AND baja = 0)
    BEGIN
        SELECT 0 AS LikeCount, 0 AS Liked;
        RETURN 1; -- comentario no existe o está dado de baja
    END

    -- devolver conteo de likes y si este usuario ya dio like
    SELECT 
        COUNT(*) AS LikeCount,
        MAX(CASE WHEN usuario_id = @usuario_id AND baja = 0 THEN 1 ELSE 0 END) AS Liked
    FROM comentario_likes
    WHERE comentario_id = @comentario_id AND baja = 0;

    RETURN 0;
END;

/*------------- BUSCADOR ------------------*/
/*----------------sp para buscar juegos---------------*/
CREATE PROCEDURE sp_GetSearchGames
    @search_term NVARCHAR(255)
AS
BEGIN
    SELECT 
        juego_id AS Id,
        nombre,
        descripcion,
        fecha_publicacion AS FechaPublicacion,
        desarrollador,
		editor,
		plataforma,
        imagenURL
    FROM juegos
    WHERE nombre LIKE '%' + @search_term + '%'
	AND baja = 0;
END;

/*------------- Calificaciones y Cantidad de reviews en GameScreen ------------------*/

CREATE PROCEDURE sp_GetCalificacionYCantReviews
	@juego_id INT,
    @TotalPuntos INT OUTPUT,
    @CantidadReviews INT OUTPUT,
    @Calificacion DECIMAL(3,1) OUTPUT
AS
BEGIN
    SELECT @TotalPuntos = SUM(puntuacion)
    FROM comentarios
    WHERE juego_id = @juego_id AND baja = 0;

    SELECT @CantidadReviews = COUNT(comentario_id)
    FROM comentarios
    WHERE juego_id = @juego_id AND baja = 0;

    IF @CantidadReviews > 0
    BEGIN
        SET @Calificacion = ROUND(CAST(@TotalPuntos AS DECIMAL(5,2)) / @CantidadReviews, 1);
        IF @Calificacion > 5.0
            SET @Calificacion = 5.0;
    END
    ELSE
    BEGIN
        SET @Calificacion = 0.0;
    END
END;

/*------------- Obtener mis reviews con usuario id ------------------*/

CREATE PROCEDURE sp_GetAllReviewsXUser
	@usuario_id INT
AS
BEGIN
	SELECT c.comentario_id, j.nombre AS nombreJuego, c.comentario, c.puntuacion FROM comentarios AS c
	JOIN juegos j ON c.juego_id = j.juego_id
	WHERE c.usuario_id = @usuario_id AND c.baja = 0 AND j.baja = 0;
END;

/*------------- Obtener top juegos para el ranking ------------------*/

CREATE PROCEDURE sp_GetTop10JuegosPorCalificacion
AS
BEGIN
    -- Tabla temporal para almacenar resultados
    CREATE TABLE #Resultados (
        juego_id INT,
        TotalPuntos INT,
        CantidadReviews INT,
        Calificacion DECIMAL(3,1)
    );

    -- Insertar datos calculados por juego
    INSERT INTO #Resultados (juego_id, TotalPuntos, CantidadReviews, Calificacion)
    SELECT 
        comentarios.juego_id,
        SUM(puntuacion) AS TotalPuntos,
        COUNT(comentario_id) AS CantidadReviews,
        CASE 
            WHEN COUNT(comentario_id) > 0 THEN 
                CASE 
                    WHEN ROUND(CAST(SUM(puntuacion) AS DECIMAL(5,2)) / COUNT(comentario_id), 1) > 5.0 THEN 5.0
                    ELSE ROUND(CAST(SUM(puntuacion) AS DECIMAL(5,2)) / COUNT(comentario_id), 1)
                END
            ELSE 0.0
        END AS Calificacion
    FROM comentarios
	JOIN juegos j ON comentarios.juego_id = j.juego_id
    WHERE comentarios.baja = 0 AND j.baja = 0
	GROUP BY comentarios.juego_id

    -- Devolver los 10 mejores
    SELECT TOP 10 
        r.juego_id,
        j.nombre AS NombreJuego,
        r.TotalPuntos,
        r.CantidadReviews,
        r.Calificacion
    FROM #Resultados r
    INNER JOIN juegos j ON r.juego_id = j.juego_id
    ORDER BY r.Calificacion DESC, r.CantidadReviews DESC;

    DROP TABLE #Resultados;
END;














































