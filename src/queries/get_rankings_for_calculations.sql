-- Give us a seminary id, and a book of the bible,
-- give me the list of rankings with the attached authorities

SELECT x.authority_id, x.book_id, x.commentary_id, x.ranking, x.seminary_id
FROM (
    SELECT r.authority_id, r.book_id, r.commentary_id, r.ranking, a.seminary_id
    FROM (
        SELECT authority_id, book_id, commentary_id, ranking
        FROM Rankings
    ) AS r
    INNER JOIN (
        SELECT id, seminary_id
        FROM Authorities
        WHERE seminary_id = {1}
    ) AS a ON r.authority_id = a.id
) AS x
INNER JOIN (
    SELECT commentary_id, book_id
    FROM CommentaryBook
    WHERE book_id = {0}
) AS y ON x.book_id = y.book_id AND x.commentary_id = y.commentary_id
ORDER BY x.commentary_id;