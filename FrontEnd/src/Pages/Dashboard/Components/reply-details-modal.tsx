import { Divider, TextField } from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Grid } from "@mui/material";
import { Card } from "@mui/material";
import CardContent from "@mui/material/CardContent";
import { IPostDetails, IThreadDetails } from "../../../Common/Interfaces";
import PostResourceCard from "./post-resource-card";

const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  height: "70%",
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

interface IProps {
  post: IPostDetails;
  thread: IThreadDetails;
}

const ReplyDetailsModal = ({ post, thread }: IProps) => {
  return (
    <div>
      <Box sx={style}>
        <Grid
          container
          justifyContent="space-between"
          direction="row"
          style={{ height: "100%" }}
        >
          <Grid item>
            <Card variant="elevation">
              <CardContent>
                <Typography variant="h6">{thread.title}</Typography>
                <Typography variant="body1">
                  by {thread.user.username} on {thread.createdDate}
                </Typography>
                <Grid container sx={{ mt: "5%", mb: "5%" }}>
                  <Typography variant="body1">
                    {thread.posts[0].description}
                  </Typography>
                </Grid>

                <PostResourceCard post={thread.posts[0]} />
              </CardContent>
            </Card>
          </Grid>

          <Card variant="elevation">
            <CardContent>
              <Typography variant="body1">
                Reply by {post.user.username} on {post.createdDate}
              </Typography>
              <Grid container sx={{ mt: "5%", mb: "5%" }}>
                <Typography variant="subtitle2">{post.description}</Typography>
              </Grid>
              <PostResourceCard post={post} />
            </CardContent>
          </Card>
        </Grid>
      </Box>
    </div>
  );
};

export default ReplyDetailsModal;
