import { Divider } from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Button } from "@mui/material";
import { Grid } from "@mui/material";
import { Card } from "@mui/material";
import CardContent from "@mui/material/CardContent";
import { IThreadDetails } from "../../../Common/Interfaces";
import PostResourceCard from "../Components/post-resource-card";

const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  height: "50%",
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

interface IProps {
  thread: IThreadDetails;
}

const PostDetailsModal = ({ thread }: IProps) => {
  return (
    <div>
      <Box sx={style}>
        <Grid
          container
          direction="column"
          justifyContent="space-between"
          style={{ height: "100%" }}
        >
          <Grid item>
            <Card variant="elevation">
              <CardContent>
                <Typography variant="h6">{thread.title}</Typography>
                <Typography variant="body1">
                  by {thread.user.username} on {thread.createdDate}
                </Typography>
                <Grid container sx={{ mt: "10%" }}>
                  <Typography variant="body1">
                    {thread.posts[0].description}
                  </Typography>
                </Grid>
              </CardContent>
            </Card>

            <Divider variant="middle" sx={{ mt: "10%" }} />
          </Grid>
          <PostResourceCard post={thread.posts[0]} />
          <Grid container justifyContent="flex-end" gap="3%">
            <Button size="small">Reply</Button>
          </Grid>
        </Grid>
      </Box>
    </div>
  );
};

export default PostDetailsModal;
