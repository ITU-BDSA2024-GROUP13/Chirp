DROP TABLE IF EXISTS user;
DROP TABLE IF EXISTS message;

create table user (
  user_id integer primary key autoincrement,
  username string not null,
  email string not null,
  pw_hash string not null
);

create table message (
  message_id integer primary key autoincrement,
  author_id integer not null,
  text string not null,
  pub_date integer
);